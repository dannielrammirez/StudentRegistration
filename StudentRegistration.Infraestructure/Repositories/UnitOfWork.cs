using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _db;
		private IStudentRepository _studentRepository;
		private ICourseRepository _courseRepository;
		private IProfessorRepository _professorRepository;
		private IEnrollmentRepository _enrollmentRepository;

		public UnitOfWork(AppDbContext db)
		{
			_db = db;
		}

		public IStudentRepository StudentRepository =>
			_studentRepository ??= new StudentRepository(_db);

		public ICourseRepository CourseRepository =>
			_courseRepository ??= new CourseRepository(_db); // Asumiendo que CourseRepository existe

		public IProfessorRepository ProfessorRepository =>
			_professorRepository ??= new ProfessorRepository(_db); // Asumiendo que ProfessorRepository existe

		public IEnrollmentRepository EnrollmentRepository =>
			_enrollmentRepository ??= new EnrollmentRepository(_db);

		public async Task<int> SaveChangesAsync()
		{
			return await _db.SaveChangesAsync();
		}

		public void Dispose()
		{
			_db.Dispose();
		}
	}
}