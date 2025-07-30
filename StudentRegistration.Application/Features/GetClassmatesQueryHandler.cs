using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;

namespace StudentRegistration.Application.Features
{
	public class GetClassmatesQueryHandler : IRequestHandler<GetClassmatesQuery, IEnumerable<StudentDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger<GetClassmatesQueryHandler> _logger;

		public GetClassmatesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetClassmatesQueryHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<IEnumerable<StudentDto>> Handle(GetClassmatesQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var account = await _unitOfWork.AccountRepository.GetByIdAsync(request.AuthenticatedAccountId) ?? throw new Exception("La cuenta de usuario no es válida.");

				var studentId = account.IdReferencia;

				var myEnrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
				var myCourseIds = myEnrollments.Select(e => e.CourseId).ToList();

				if (!myCourseIds.Any())
					return new List<StudentDto>();

				var classmatesEnrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByCourseIdsAsync(myCourseIds);

				var classmateIds = classmatesEnrollments
					.Where(e => e.StudentId != request.AuthenticatedAccountId)
					.Select(e => e.StudentId)
					.Distinct()
					.ToList();

				if (!classmateIds.Any())
					return new List<StudentDto>();

				var classmates = await _unitOfWork.StudentRepository.GetByIdsAsync(classmateIds);

				return _mapper.Map<IEnumerable<StudentDto>>(classmates);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ocurrió un error en el proceso de obtener de materias para los estudiantes. Detalles: {ex.Message}", ex);
				throw;
			}
		}
	}
}