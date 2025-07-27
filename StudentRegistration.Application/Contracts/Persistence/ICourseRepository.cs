using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface ICourseRepository
	{
		Task<Course> GetByIdAsync(Guid id);
		Task<IEnumerable<Course>> GetAllAsync();
	}
}