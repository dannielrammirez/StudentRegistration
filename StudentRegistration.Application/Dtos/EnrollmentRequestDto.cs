namespace StudentRegistration.Application.Dtos
{
	public class EnrollmentRequestDto
	{
		public Guid StudentId { get; set; }
		public List<Guid> CourseIds { get; set; }
	}
}