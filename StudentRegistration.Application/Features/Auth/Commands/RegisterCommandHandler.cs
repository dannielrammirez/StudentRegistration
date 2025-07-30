using MediatR;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Enums;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Enums;
using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ISecurityService _securityService;
	private readonly ILogger<RegisterCommandHandler> _logger;

	public RegisterCommandHandler(IUnitOfWork unitOfWork, ISecurityService securityService, ILogger<RegisterCommandHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_securityService = securityService;
		_logger = logger;
	}

	public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var existingAccount = await _unitOfWork.AccountRepository.ValidateExist(request.Email);

			if (existingAccount)
				throw new DuplicateEmailException($"El correo electrónico '{request.Email}' ya está registrado.");

			string baseUsername = $"{request.FirstName.ToLower()[0]}{request.LastName.ToLower()}".Replace(" ", "");
			int existingCount = await _unitOfWork.AccountRepository.CountSimilarUsernamesAsync(baseUsername);
			string finalUsername = $"{baseUsername}{(existingCount > 0 ? (existingCount + 1).ToString() : "")}";

			var student = new Student
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
			};

			await _unitOfWork.StudentRepository.AddAsync(student);

			_securityService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

			var account = new Account
			{
				Email = request.Email,
				Username = finalUsername,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				IdReferencia = student.Id,
				TipoCuenta = EnumTipoCuenta.Estudiante
			};

			await _unitOfWork.AccountRepository.AddAsync(account);

			await _unitOfWork.SaveChangesAsync();

			return student.Id;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Ocurrió un error en el proceso de registro para el email {Email}", request.Email);
			throw;
		}
	}
}