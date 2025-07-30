using MediatR;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Features.Auth.Commands
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ISecurityService _securityService;
		private readonly ILogger<LoginCommandHandler> _logger;

		public LoginCommandHandler(IUnitOfWork unitOfWork, ISecurityService securityService, ILogger<LoginCommandHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_securityService = securityService;
			_logger = logger;
		}

		public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			try
			{
				Account objAccount;

				if (request.LoginIdentifier.Contains("@"))
					objAccount = await _unitOfWork.AccountRepository.GetByEmailAsync(request.LoginIdentifier);
				else
					objAccount = await _unitOfWork.AccountRepository.GetByUsernameAsync(request.LoginIdentifier);

				if (objAccount == null || !_securityService.VerifyPasswordHash(request.Password, objAccount.PasswordHash, objAccount.PasswordSalt))
					throw new Exception("Credenciales inválidas.");

				var accessToken = _securityService.GenerateJwtToken(objAccount);

				var refreshToken = _securityService.GenerateRefreshToken();

				refreshToken.IdCuenta = objAccount.Id;

				await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
				await _unitOfWork.SaveChangesAsync();

				return new LoginResponseDto
				{
					AccessToken = accessToken,
					RefreshToken = refreshToken.Token
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ocurrió un error en el proceso de registro para {Email}", request.LoginIdentifier);
				throw;
			}
		}
	}
}