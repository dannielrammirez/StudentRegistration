using MediatR;
using StudentRegistration.Application.Dtos;

namespace StudentRegistration.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<LoginResponseDto>
{
	public string LoginIdentifier { get; set; }
	public string Password { get; set; }
}