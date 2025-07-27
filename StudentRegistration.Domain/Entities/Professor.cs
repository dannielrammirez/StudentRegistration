using StudentRegistration.Domain.Entities.Common;

namespace StudentRegistration.Domain.Entities
{
	public class Professor : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public ICollection<Course> Courses { get; set; } = new List<Course>();
	}
}