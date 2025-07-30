namespace StudentRegistration.Application.Dtos
{
	public class LogErroresDto
	{
		public Guid Id { get; set; }
		public Guid IdPuntoVenta { get; set; }
		public string? Action { get; set; }
		public string? Detalle { get; set; }
		public string? Error { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}