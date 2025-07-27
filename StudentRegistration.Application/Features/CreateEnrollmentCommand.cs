using MediatR;

namespace StudentRegistration.Application.Features
{
	public class CreateEnrollmentCommand : IRequest<Guid>
	{
		public Guid StudentId { get; set; }
		public Guid CourseId { get; set; }
	}
}