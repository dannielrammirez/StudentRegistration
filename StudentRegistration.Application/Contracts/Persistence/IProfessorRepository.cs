using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface IProfessorRepository
	{
		Task<Professor> GetByIdAsync(Guid id);
	}
}