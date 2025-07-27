using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Presentation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EnrollmentsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public EnrollmentsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[HttpPost]
		public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentRequestDto enrollmentRequest)
		{
			var student = await _unitOfWork.StudentRepository.GetByIdAsync(enrollmentRequest.StudentId);

			if (student == null)
				return NotFound("Estudiante no encontrado.");

			var courseToEnroll = await _unitOfWork.CourseRepository.GetByIdAsync(enrollmentRequest.CourseId);
			
			if (courseToEnroll == null)
				return NotFound("Materia no encontrada.");
			
			var currentEnrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(student.Id);

			if (currentEnrollments.Count() >= 3)
				return BadRequest("Límite de 3 materias alcanzado.");

			var studentProfessorIds = currentEnrollments.Select(e => e.Course.ProfessorId).ToList();

			if (studentProfessorIds.Contains(courseToEnroll.ProfessorId))
				return BadRequest("Ya tienes una materia con este profesor.");

			var newEnrollment = new Enrollment
			{
				StudentId = student.Id,
				CourseId = courseToEnroll.Id
			};

			await _unitOfWork.EnrollmentRepository.AddAsync(newEnrollment);
			await _unitOfWork.SaveChangesAsync();

			return Ok(newEnrollment);
		}
	}
}