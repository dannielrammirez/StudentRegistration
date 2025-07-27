namespace StudentRegistration.Application.Dtos
{
	public class CourseDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Credits { get; set; }
		public ProfessorDto Professor { get; set; }
	}
}