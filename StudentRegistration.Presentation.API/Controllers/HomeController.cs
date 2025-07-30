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
	public class HomeController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCourses()
		{
			var courses = await _unitOfWork.CourseRepository.GetAllAsync();
			var coursesDto = _mapper.Map<IEnumerable<CourseDto>>(courses);

			return Ok(coursesDto);
		}

		[HttpGet("{courseId}/classmates")]
		public async Task<IActionResult> GetClassmates(Guid courseId)
		{
			var enrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByCourseIdAsync(courseId);
			var classmatesDto = _mapper.Map<IEnumerable<ClassmateDto>>(enrollments);

			return Ok(classmatesDto);
		}
	}
}