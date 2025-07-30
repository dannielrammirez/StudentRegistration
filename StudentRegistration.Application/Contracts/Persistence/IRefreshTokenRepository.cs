using StudentRegistration.Application.Dtos;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface IRefreshTokenRepository
	{
		Task<bool> AddAsync(RefreshToken refreshToken);
		Task<RefreshToken?> GetByTokenAsync(string vlrJwt);
		Task<List<RefreshToken>> GetAllByAccountIdAsync(Guid IdAccount);
		Task<bool> SaveRotatedAsync(RefreshToken revokedToken, RefreshToken newToken);
		Task<bool> UpdateAsync(RefreshToken refreshToken);
	}
}