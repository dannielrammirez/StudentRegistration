using MediatR;

namespace StudentRegistration.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<Guid>
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
}