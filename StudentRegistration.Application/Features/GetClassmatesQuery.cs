using MediatR;
using StudentRegistration.Application.Dtos;

namespace StudentRegistration.Application.Features
{
	public class GetClassmatesQuery : IRequest<IEnumerable<StudentDto>>
	{
		public Guid AuthenticatedStudentId { get; set; }
	}
}
