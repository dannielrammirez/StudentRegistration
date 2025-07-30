using StudentRegistration.Domain.Entities.Common;
using StudentRegistration.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRegistration.Domain.Entities
{
	public class RefreshToken : BaseEntity
	{
		public string Token { get; set; }
		public DateTime Expires { get; set; }
		public DateTime? Revoked { get; set; }
		public bool IsRevoked { get; set; }
		public string? ReplacedByToken { get; set; }
		public Guid IdCuenta { get; set; }
		[ForeignKey("IdCuenta")]
		public Account Cuenta { get; set; }
		public EnumTipoCuenta IdTipoCuenta{ get; set; }
	}
}