using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class EnrollmentRepository : IEnrollmentRepository
	{
		private readonly AppDbContext _db;

		public EnrollmentRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task AddAsync(Enrollment enrollment)
		{
			await _db.Enrollments.AddAsync(enrollment);
		}

		public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(Guid studentId)
		{
			var respnse = await _db.Enrollments
				.Where(e => e.StudentId == studentId && e.IsActive)
				.Include(e => e.Course)
					.ThenInclude(c => c.Professor)
				.ToListAsync();

			return respnse;
		}

		public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdAsync(Guid courseId)
		{
			var response = await _db.Enrollments
				.Where(e => e.CourseId == courseId && e.IsActive)
				.Include(e => e.Student)
				.ToListAsync();

			return response;
		}

		public async Task AddRangeAsync(IEnumerable<Enrollment> enrollments)
		{
			await _db.Enrollments.AddRangeAsync(enrollments);
		}

		public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdsAsync(List<Guid> courseIds)
		{
			var response = await _db.Enrollments
				.Where(e => courseIds.Contains(e.CourseId) && e.IsActive)
				.ToListAsync();

			return response;
		}
	}
}