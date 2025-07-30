using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Application.Features;
using System.Security.Claims;

namespace StudentRegistration.Presentation.API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class StudentsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		public StudentsController(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_mediator = mediator;
		}

		/// <summary>
		/// Obtiene la lista de todos los estudiantes registrados en el sistema.
		/// </summary>
		/// <returns>Una lista de estudiantes.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetAllStudents()
		{
			var students = await _unitOfWork.StudentRepository.GetAllAsync();
			var studentsDto = _mapper.Map<IEnumerable<StudentDto>>(students);
			return Ok(studentsDto);
		}
		/// <summary>
		/// Obtiene las materias en las que un estudiante específico está inscrito.
		/// </summary>
		/// <param name="studentId">El ID del estudiante.</param>
		/// <returns>Una lista de las materias inscritas.</returns>
		[HttpGet("{studentId}/enrollments")]
		[ProducesResponseType(typeof(IEnumerable<EnrolledCourseDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetStudentEnrollments(Guid studentId)
		{
			var enrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
			var enrolledCoursesDto = _mapper.Map<IEnumerable<EnrolledCourseDto>>(enrollments);
			return Ok(enrolledCoursesDto);
		}

		/// <summary>
		/// Obtiene la lista de compañeros de clase del estudiante autenticado.
		/// </summary>
		/// <returns>Una lista de estudiantes que comparten al menos una materia.</returns>
		[HttpGet("classmates")]
		[ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetMyClassmates()
		{
			var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var query = new GetClassmatesQuery { AuthenticatedStudentId = studentId };

			var classmates = await _mediator.Send(query);

			return Ok(classmates);
		}
	}
}