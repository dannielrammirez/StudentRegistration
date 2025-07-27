using StudentRegistration.Domain.Entities.Common;

namespace StudentRegistration.Domain.Entities
{
	public class Student : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
	}
}