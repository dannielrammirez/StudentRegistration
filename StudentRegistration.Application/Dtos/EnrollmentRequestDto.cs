namespace StudentRegistration.Application.Dtos
{
	public class EnrollmentRequestDto
	{
		public Guid StudentId { get; set; }
		public Guid CourseId { get; set; }
	}
}