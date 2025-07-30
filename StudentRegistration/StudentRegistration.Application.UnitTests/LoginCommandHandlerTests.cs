using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Features.Auth.Commands;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.UnitTests.Features.Auth;

public class LoginCommandHandlerTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<ISecurityService> _securityServiceMock;
	private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
	private readonly Mock<IAccountRepository> _accountRepositoryMock;
	private readonly LoginCommandHandler _handler;

	public LoginCommandHandlerTests()
	{
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_securityServiceMock = new Mock<ISecurityService>();
		_loggerMock = new Mock<ILogger<LoginCommandHandler>>();
		_accountRepositoryMock = new Mock<IAccountRepository>();

		_unitOfWorkMock.Setup(u => u.AccountRepository).Returns(_accountRepositoryMock.Object);

		_handler = new LoginCommandHandler(_unitOfWorkMock.Object, _securityServiceMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ShouldReturnLoginResponse_WhenCredentialsAreValid()
	{
		var command = new LoginCommand { LoginIdentifier = "test@test.com", Password = "password123" };
		var account = new Account { Email = "test@test.com" };
		var fakeToken = "fake.jwt.token";

		_accountRepositoryMock.Setup(r => r.GetByEmailAsync(command.LoginIdentifier)).ReturnsAsync(account);
		_securityServiceMock.Setup(s => s.VerifyPasswordHash(command.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
		_securityServiceMock.Setup(s => s.GenerateJwtToken(account)).Returns(fakeToken);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().NotBeNull();
		result.AccessToken.Should().Be(fakeToken);
	}

	[Fact]
	public async Task Handle_ShouldThrowException_WhenAccountNotFound()
	{
		var command = new LoginCommand { LoginIdentifier = "nouser@test.com", Password = "password123" };

		_accountRepositoryMock.Setup(r => r.GetByEmailAsync(command.LoginIdentifier)).ReturnsAsync((Account)null);

		Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

		await act.Should().ThrowAsync<Exception>().WithMessage("Credenciales inválidas.");
	}
}