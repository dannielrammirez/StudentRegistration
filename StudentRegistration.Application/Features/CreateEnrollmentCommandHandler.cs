using MediatR;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using System.Security.Principal;

namespace StudentRegistration.Application.Features
{
	public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<CreateEnrollmentCommandHandler> _logger;

		public CreateEnrollmentCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateEnrollmentCommandHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if (request.CourseIds.Count != 3)
					throw new Exception("Debe seleccionar exactamente 3 materias para inscribirse.");

				var account = await _unitOfWork.AccountRepository.GetByIdAsync(request.AuthenticatedAccountId) ?? throw new Exception("La cuenta de usuario no es válida.");
				
				var studentId = account.IdReferencia;

				var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId) ?? throw new Exception($"Estudiante con ID {studentId} no encontrado.");

				var currentEnrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);

				var selectedCourses = new List<Course>();
				foreach (var courseId in request.CourseIds)
				{
					var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId) ?? throw new Exception($"Materia con ID {courseId} no encontrada.");
					selectedCourses.Add(course);
				}

				var existingProfessorIds = currentEnrollments.Select(e => e.Course.ProfessorId);
				var newProfessorIds = selectedCourses.Select(c => c.ProfessorId);
				var allProfessorIds = existingProfessorIds.Concat(newProfessorIds).ToList();

				if (allProfessorIds.Count != allProfessorIds.Distinct().Count())
					throw new Exception("No puedes inscribir dos materias con el mismo profesor.");

				var newEnrollments = request.CourseIds.Select(courseId => new Enrollment
				{
					StudentId = studentId,
					CourseId = courseId
				}).ToList();

				await _unitOfWork.EnrollmentRepository.AddRangeAsync(newEnrollments);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ocurrió un error en el proceso de asignacion de materias para el estudiante {request.AuthenticatedStudentId}", request.AuthenticatedAccountId);
				throw;
			}
		}
	}
}