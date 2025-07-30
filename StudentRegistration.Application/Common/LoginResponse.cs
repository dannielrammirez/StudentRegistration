using StudentRegistration.Domain.Enums;

namespace StudentRegistration.Application.Common
{
	public class LoginResponse
	{
		public Guid IdCuenta { get; set; }
		public string Username { get; set; }
		public string Nombre { get; set; }
		public string AccessToken { get; init; }
		public string RefreshToken { get; init; }
		public DateTime AccessTokenExpires { get; init; }
		public DateTime RefreshTokenExpires { get; init; }
		public string TipoCuenta { get; init; }
		public string UserNombre{ get; init; }
		public string CuentaEmail { get; init; }
		public string EstadoCuenta { get; init; }
	}
}