using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface IEnrollmentRepository
	{
		Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdAsync(Guid courseId);
		Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(Guid studentId);
		Task AddAsync(Enrollment enrollment);
		Task AddRangeAsync(IEnumerable<Enrollment> enrollments);
		Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdsAsync(List<Guid> courseIds);
	}
}