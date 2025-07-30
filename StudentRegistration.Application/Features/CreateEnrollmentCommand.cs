using MediatR;

namespace StudentRegistration.Application.Features
{
	public class CreateEnrollmentCommand : IRequest
	{
		public Guid AuthenticatedAccountId { get; set; }
		public List<Guid> CourseIds { get; set; }
	}
}