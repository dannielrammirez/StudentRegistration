using StudentRegistration.Application.Enums;

namespace StudentRegistration.Application.Dtos
{
	public record AuthResponseDto
	{
		public string AccessToken { get; init; }
		public DateTime AccessTokenExpires { get; init; }
		public string RefreshToken { get; init; }
		public DateTime RefreshTokenExpires { get; init; }
		public string TipoCliente { get; init; }
		public string Username { get; init; }
		public EnumEstadoCuenta EstadoCuenta { get; init; }
		public Guid IdCuenta { get; init; }
		public string? CuentaEmail { get; set; }
		public string? LoginIdentifier { get; set; }
		public string? Password { get; set; }
	}
}