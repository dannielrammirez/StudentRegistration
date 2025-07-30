using System.ComponentModel;

namespace StudentRegistration.Domain.Enums
{
	public enum EnumTipoCuenta
	{
		[Description("SuperAdministrador")]
		SuperAdministrador = 1,
		[Description("Profesor")]
		Profesor = 2,
		[Description("Estudiante")]
		Estudiante = 3
	}
}