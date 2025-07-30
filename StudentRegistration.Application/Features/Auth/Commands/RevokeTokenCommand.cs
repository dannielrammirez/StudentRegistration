using MediatR;
using StudentRegistration.Application.Dtos;

namespace StudentRegistration.Application.Features.Auth.Commands
{
	public class RevokeTokenCommand : IRequest
	{
		public Guid AccountId { get; set; }
	}
}