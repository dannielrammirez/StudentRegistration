using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;

namespace StudentRegistration.Presentation.API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CoursesController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CoursesController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		/// <summary>
		/// Obtiene la lista completa de materias disponibles.
		/// </summary>
		/// <returns>Una lista de materias con sus profesores.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetAllCourses()
		{
			var courses = await _unitOfWork.CourseRepository.GetAllAsync();
			var coursesDto = _mapper.Map<IEnumerable<CourseDto>>(courses);
			return Ok(coursesDto);
		}

		/// <summary>
		/// Obtiene la lista de compañeros inscritos en una materia específica.
		/// </summary>
		/// <param name="courseId">El ID de la materia a consultar.</param>
		/// <returns>Una lista con los nombres de los estudiantes inscritos.</returns>
		[HttpGet("{courseId}/classmates")]
		[ProducesResponseType(typeof(IEnumerable<ClassmateDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetClassmates(Guid courseId)
		{
			var enrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByCourseIdAsync(courseId);
			var classmatesDto = _mapper.Map<IEnumerable<ClassmateDto>>(enrollments);
			return Ok(classmatesDto);
		}
	}
}