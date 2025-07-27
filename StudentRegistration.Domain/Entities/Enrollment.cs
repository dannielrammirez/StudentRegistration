using StudentRegistration.Domain.Entities.Common;

namespace StudentRegistration.Domain.Entities
{
	public class Enrollment : BaseEntity
	{
		public Guid StudentId { get; set; }
		public Guid CourseId { get; set; }
		public Student Student { get; set; }
		public Course Course { get; set; }
	}
}