using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface IStudentRepository
	{
		Task<Student> GetByIdAsync(Guid id);
		Task<Student> GetByEmailAsync(string email);
		Task<IEnumerable<Student>> GetAllAsync();
		Task AddAsync(Student student);
		Task<IEnumerable<Student>> GetByIdsAsync(List<Guid> ids);
	}
}