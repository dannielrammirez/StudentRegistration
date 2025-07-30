using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace StudentRegistration.Application.Common
{
	public static class SecurityHelper
	{
		public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
		{
			using var hmac = new HMACSHA512(storedSalt);
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

			for (int i = 0; i < computedHash.Length; i++)
			{
				if (computedHash[i] != storedHash[i]) return false;
			}

			return true;
		}

		public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		public static string GenerateJwtToken(string key, Guid cuentaId, string email)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var keyBytes = Encoding.UTF8.GetBytes(key);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, cuentaId.ToString()),
					new Claim(ClaimTypes.Name, email)
				}),
				Expires = DateTime.UtcNow.AddHours(12),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha512Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}