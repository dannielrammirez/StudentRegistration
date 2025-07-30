using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly AppDbContext _context;

		public RefreshTokenRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<bool> AddAsync(RefreshToken refreshToken)
		{
			_context.RefreshToken.Add(refreshToken);

			var response = await SaveAsync();
			return response;
		}

		public async Task<RefreshToken?> GetByTokenAsync(string token)
		{
			var response = await _context.RefreshToken.FirstOrDefaultAsync(t => t.Token == token);

			return response;
		}

		public async Task<List<RefreshToken>> GetAllByAccountIdAsync(Guid IdAccount)
		{
			var response = await _context.RefreshToken
				.Where(rt => rt.IdCuenta == IdAccount)
				.ToListAsync();

			return response;
		}

		public async Task<bool> SaveRotatedAsync(RefreshToken revokedToken, RefreshToken newToken)
		{
			_context.RefreshToken.Update(revokedToken);
			_context.RefreshToken.Add(newToken);

			var response = await SaveAsync();
			return response;
		}

		public async Task<bool> UpdateAsync(RefreshToken refreshToken)
		{
			_context.RefreshToken.Update(refreshToken);

			var response = await SaveAsync();

			return response;
		}

		private async Task<bool> SaveAsync()
		{
			var response = await _context.SaveChangesAsync() > 0;
			return response;
		}
	}
}