namespace StudentRegistration.Application.Dtos
{
	public class AccountDto
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string TipoCuenta { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}