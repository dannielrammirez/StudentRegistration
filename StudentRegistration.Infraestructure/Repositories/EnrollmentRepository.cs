using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class EnrollmentRepository : IEnrollmentRepository
	{
		private readonly AppDbContext _context;

		public EnrollmentRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Enrollment enrollment)
		{
			await _context.Enrollments.AddAsync(enrollment);
		}

		public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(Guid studentId)
		{
			return await _context.Enrollments
				.Where(e => e.StudentId == studentId && e.IsActive)
				.Include(e => e.Course)
					.ThenInclude(c => c.Professor)
				.ToListAsync();
		}

		public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdAsync(Guid courseId)
		{
			return await _context.Enrollments
				.Where(e => e.CourseId == courseId && e.IsActive)
				.Include(e => e.Student)
				.ToListAsync();
		}
	}
}