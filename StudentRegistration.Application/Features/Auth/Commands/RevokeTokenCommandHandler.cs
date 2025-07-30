using MediatR;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Contracts.Persistence;

namespace StudentRegistration.Application.Features.Auth.Commands
{
	public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<RevokeTokenCommandHandler> _logger;
		public RevokeTokenCommandHandler(IUnitOfWork unitOfWork, ILogger<RevokeTokenCommandHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var tokens = await _unitOfWork.RefreshTokenRepository.GetAllByAccountIdAsync(request.AccountId);

				foreach (var token in tokens)
				{
					token.Revoked = DateTime.UtcNow;
					token.IsRevoked = true;
				}

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ocurrió un error en el proceso de revocacion de token para {AccountId}", request.AccountId);
				throw;
			}
		}
	}
}