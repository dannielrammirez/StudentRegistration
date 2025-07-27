using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class StudentRepository : IStudentRepository
	{
		private readonly AppDbContext _context;

		public StudentRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Student> GetByIdAsync(Guid id)
		{
			return await _context.Students.FindAsync(id);
		}

		public async Task<Student> GetByEmailAsync(string email)
		{
			return await _context.Students
				.FirstOrDefaultAsync(s => s.Email == email && s.IsActive);
		}
		public async Task<IEnumerable<Student>> GetAllAsync()
		{
			return await _context.Students
				.Where(s => s.IsActive)
				.ToListAsync();
		}

		public async Task AddAsync(Student student)
		{
			await _context.Students.AddAsync(student);
		}
	}
}