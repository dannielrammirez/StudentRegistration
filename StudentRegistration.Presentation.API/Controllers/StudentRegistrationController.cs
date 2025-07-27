using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;

namespace StudentRegistration.Presentation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StudentsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public StudentsController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllStudents()
		{
			var students = await _unitOfWork.StudentRepository.GetAllAsync();
			var studentsDto = _mapper.Map<IEnumerable<StudentDto>>(students);

			return Ok(studentsDto);
		}
	}
}