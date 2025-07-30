using StudentRegistration.Domain.Entities.Common;
using StudentRegistration.Domain.Enums;

namespace StudentRegistration.Domain.Entities
{
	public class Account : BaseEntity
	{
		public string Username { get; set; } = null!;
		public string Email { get; set; } = null!;
		public byte[] PasswordHash { get; set; } = null!;
		public byte[] PasswordSalt { get; set; } = null!;
		public Guid IdReferencia { get; set; } = Guid.Empty!;
		public EnumTipoCuenta TipoCuenta { get; set; }
		public int IdEstado { get; set; }
	}
}