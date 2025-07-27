namespace StudentRegistration.Application.Features
{
	using MediatR;
	using StudentRegistration.Application.Contracts.Persistence;
	using StudentRegistration.Domain.Entities;
	using StudentRegistration.Domain.Exceptions;

	public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateEnrollmentCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Guid> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
		{
			var student = await _unitOfWork.StudentRepository.GetByIdAsync(request.StudentId);
			if (student == null) throw new Exception("Estudiante no encontrado."); // O una excepción personalizada

			var courseToEnroll = await _unitOfWork.CourseRepository.GetByIdAsync(request.CourseId);
			if (courseToEnroll == null) throw new Exception("Materia no encontrada.");

			var currentEnrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(student.Id);

			if (currentEnrollments.Count() >= 3)
			{
				throw new EnrollmentRuleException("Límite de 3 materias alcanzado.");
			}

			var studentProfessorIds = currentEnrollments.Select(e => e.Course.ProfessorId).ToList();
			if (studentProfessorIds.Contains(courseToEnroll.ProfessorId))
			{
				throw new EnrollmentRuleException("Ya tienes una materia con este profesor.");
			}

			var newEnrollment = new Enrollment
			{
				StudentId = student.Id,
				CourseId = courseToEnroll.Id
			};

			await _unitOfWork.EnrollmentRepository.AddAsync(newEnrollment);
			await _unitOfWork.SaveChangesAsync();

			return newEnrollment.Id;
		}
	}
}