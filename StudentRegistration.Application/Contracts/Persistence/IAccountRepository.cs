using StudentRegistration.Application.Dtos;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface IAccountRepository
	{
		Task AddAsync(Account cuenta);
		Task<Account> GetByUsernameAsync(string username);
		Task<bool> ValidateExist(string email);
		Task<Account> GetByEmailAsync(string email);
		Task<Account> GetByIdAsync(Guid id);
		Task<ICollection<AccountDto>> ListAsync();
		Task UpdateAsync(Account cuenta);
		Task<int> CountSimilarUsernamesAsync(string baseUsername);
	}
}