using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class AccountRepository : IAccountRepository
	{
		private readonly AppDbContext _db;
		public AccountRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task AddAsync(Account cuenta)
		{
			await _db.Account.AddAsync(cuenta);
		}

		public async Task<Account> GetByEmailAsync(string email)
		{
			var response = await _db.Account
				.Where(c => c.Email == email)
				.Select(c => new Account
				{
					Id = c.Id,
					Username = c.Username,
					Email = c.Email,
					CreatedAt = c.CreatedAt,
					PasswordHash = c.PasswordHash,
					PasswordSalt = c.PasswordSalt,
					UpdatedAt = c.UpdatedAt
				})
				.FirstOrDefaultAsync();

			return response;
		}

		public async Task<Account> GetByUsernameAsync(string userName)
		{
			var response = await _db.Account
				.Where(c => c.Username == userName)
				.Select(c => new Account
				{
					Id = c.Id,
					Username = c.Username,
					Email = c.Email,
					PasswordHash = c.PasswordHash,
					PasswordSalt = c.PasswordSalt,
					CreatedAt = c.CreatedAt,
					UpdatedAt = c.UpdatedAt
				})
				.FirstOrDefaultAsync();

			return response;
		}

		public async Task<Account> GetByIdAsync(Guid id)
		{
			var response = await _db.Account
				.Where(c => c.Id == id)
				.Select(c => new Account
				{
					Id = c.Id,
					Username = c.Username,
					Email = c.Email,
					IdReferencia = c.IdReferencia,
					CreatedAt = c.CreatedAt,
					UpdatedAt = c.UpdatedAt
				})
				.FirstOrDefaultAsync();

			return response;
		}

		public async Task<ICollection<AccountDto>> ListAsync()
		{
			var response = await _db.Account
				.Select(c => new AccountDto
				{
					Id = c.Id,
					Username = c.Username,
					Email = c.Email,
					CreatedAt = c.CreatedAt,
					UpdatedAt = c.UpdatedAt
				})
				.ToListAsync();

			if (response == null)
				throw new Exception("No se encontraron registros.");

			return response;
		}

		public async Task UpdateAsync(Account cuenta)
		{
			_db.Account.Update(cuenta);
		}

		public async Task<bool> ValidateExist(string email)
		{
			var reponse = await _db.Account.AnyAsync(c => c.Email == email);
			return reponse;
		}

		public async Task<int> CountSimilarUsernamesAsync(string baseUsername)
		{
			var response = await _db.Account.CountAsync(a => a.Username.StartsWith(baseUsername));
			return response;
		}
	}
}