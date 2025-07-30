using MediatR;
using StudentRegistration.Application.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Contracts.Persistence;

namespace StudentRegistration.Application.Features.Auth.Commands
{
	public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ISecurityService _securityService;
		private readonly ILogger<RefreshTokenCommandHandler> _logger;

		public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, ISecurityService securityService, ILogger<RefreshTokenCommandHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_securityService = securityService;
			_logger = logger;
		}

		public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var principal = _securityService.GetPrincipalFromExpiredToken(request.AccessToken);
				var accountIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(accountIdString))
					throw new Exception("Invalid token: Missing NameIdentifier claim.");

				var accountId = Guid.Parse(accountIdString);

				var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
				var savedRefreshToken = await _unitOfWork.RefreshTokenRepository.GetByTokenAsync(request.RefreshToken);

				if (account is null || savedRefreshToken is null || savedRefreshToken.IdCuenta != account.Id || savedRefreshToken.Expires <= DateTime.UtcNow || savedRefreshToken.IsRevoked)
				{
					throw new Exception("Invalid client request");
				}

				var newAccessToken = _securityService.GenerateJwtToken(account);
				var newRefreshToken = _securityService.GenerateRefreshToken();

				savedRefreshToken.Revoked = DateTime.UtcNow;
				savedRefreshToken.ReplacedByToken = newRefreshToken.Token;

				newRefreshToken.IdCuenta = account.Id;
				await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);

				await _unitOfWork.SaveChangesAsync();

				return new LoginResponseDto
				{
					AccessToken = newAccessToken,
					RefreshToken = newRefreshToken.Token
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ocurrió un error en el proceso de actualización de token. Detalles: {ex.Message}", ex);
				throw;
			}
		}
	}
}