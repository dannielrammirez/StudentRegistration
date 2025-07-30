using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentRegistration.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentRegistration.Application.Common
{
	public static class JwtHelper
	{
		public static (string AccessToken, DateTime ExpiresAt) GenerateJwtToken(Account cuenta, string vlrJwt)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(vlrJwt));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiresAt = DateTime.UtcNow.AddMinutes(60);

			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, cuenta.Id.ToString()),
				new(ClaimTypes.Email, cuenta.Email),
				new("Username", cuenta.Username)
			};

			claims.Add(new Claim("TipoCuenta", cuenta.TipoCuenta.ObtenerDescripcion()));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = expiresAt,
				SigningCredentials = creds,
				Issuer = "https://www.pruebatecnicainterrapidisimoingsenior.net",
				Audience = $"StudentRegisterAudience"
			};

			var handler = new JwtSecurityTokenHandler();
			var securityToken = handler.CreateToken(tokenDescriptor);
			var accessToken = handler.WriteToken(securityToken);

			return (accessToken, expiresAt);
		}

		public static SecurityToken GetJwtSecurityTokenHandler(JwtSecurityTokenHandler tokenHandler, string key, Account objAccount)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, objAccount.Id.ToString()),
				new Claim(ClaimTypes.Name, objAccount.Email.ToString()),
			};

			var skey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var credenciales = new SigningCredentials(skey, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = credenciales
			};

			return tokenHandler.CreateToken(tokenDescriptor);
		}
	}
}