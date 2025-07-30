using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class StudentRepository : IStudentRepository
	{
		private readonly AppDbContext _db;

		public StudentRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task<Student> GetByIdAsync(Guid id)
		{
			var response = await _db.Students.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
			return response;
		}

		public async Task<Student> GetByEmailAsync(string email)
		{
			var response = await _db.Students
				.FirstOrDefaultAsync(s => s.Email == email && s.IsActive);

			return response;
		}
		public async Task<IEnumerable<Student>> GetAllAsync()
		{
			var response = await _db.Students
				.Where(s => s.IsActive)
				.ToListAsync();

			return response;
		}

		public async Task AddAsync(Student student)
		{
			await _db.Students.AddAsync(student);
		}

		public async Task<IEnumerable<Student>> GetByIdsAsync(List<Guid> ids)
		{
			var response = await _db.Students
				.Where(s => ids.Contains(s.Id) && s.IsActive)
				.ToListAsync();

			return response;
		}
	}
}