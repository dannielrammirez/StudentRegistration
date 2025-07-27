using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class CourseRepository : ICourseRepository
	{
		private readonly AppDbContext _context;

		public CourseRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Course> GetByIdAsync(Guid id)
		{
			return await _context.Courses.FindAsync(id);
		}

		public async Task<IEnumerable<Course>> GetAllAsync()
		{
			return await _context.Courses
				.Where(c => c.IsActive)
				.Include(c => c.Professor)
				.ToListAsync();
		}
	}
}