using StudentRegistration.Domain.Entities.Common;

namespace StudentRegistration.Domain.Entities
{
	public class LogErrores : BaseEntity
	{
		public string? Action { get; set; }
		public string? Detalle { get; set; }
		public string? Error { get; set; }
	}
}
