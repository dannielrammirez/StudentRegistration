using StudentRegistration.Domain.Entities;
using System.Security.Claims;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface ISecurityService
	{
		void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
		bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
		string GenerateJwtToken(Account account);
		RefreshToken GenerateRefreshToken();
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}