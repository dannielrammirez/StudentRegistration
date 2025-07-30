using MediatR;
using StudentRegistration.Application.Dtos;

namespace StudentRegistration.Application.Features.Auth.Commands
{
	public class RefreshTokenCommand : IRequest<LoginResponseDto>
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}