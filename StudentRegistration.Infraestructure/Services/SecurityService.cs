using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudentRegistration.Infrastructure.Services
{
	public class SecurityService : ISecurityService
	{
		private readonly IConfiguration _config;

		public SecurityService(IConfiguration conf)
		{
			_config = conf;
		}

		public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}

		public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}

		public string GenerateJwtToken(Account account)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
				new Claim(ClaimTypes.Email, account.Email),
				new Claim(ClaimTypes.Name, account.Username)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var duration = double.Parse(_config["Jwt:DurationInMinutes"]);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(duration),
				Issuer = _config["Jwt:Issuer"],
				Audience = _config["Jwt:Audience"],
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			var response = tokenHandler.WriteToken(token);

			return response;
		}

		public RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			var response = new RefreshToken
			{
				Token = Convert.ToBase64String(randomNumber),
				Expires = DateTime.UtcNow.AddDays(7),
			};

			return response;
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
				ValidateLifetime = false
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}