using StudentRegistration.Domain.Entities.Common;

namespace StudentRegistration.Domain.Entities
{
	public class Course : BaseEntity
	{
		public string Name { get; set; }
		public int Credits { get; set; }
		public Guid ProfessorId { get; set; }
		public Professor Professor { get; set; }
		public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
	}
}