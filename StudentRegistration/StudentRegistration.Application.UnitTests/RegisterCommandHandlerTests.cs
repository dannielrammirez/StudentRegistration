using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Features.Auth.Commands;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Application.UnitTests.Features.Auth;

public class RegisterCommandHandlerTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<ISecurityService> _securityServiceMock;
	private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
	private readonly Mock<IAccountRepository> _accountRepositoryMock;
	private readonly Mock<IStudentRepository> _studentRepositoryMock;
	private readonly RegisterCommandHandler _handler;

	public RegisterCommandHandlerTests()
	{
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_securityServiceMock = new Mock<ISecurityService>();
		_loggerMock = new Mock<ILogger<RegisterCommandHandler>>();
		_accountRepositoryMock = new Mock<IAccountRepository>();
		_studentRepositoryMock = new Mock<IStudentRepository>();

		_unitOfWorkMock.Setup(u => u.AccountRepository).Returns(_accountRepositoryMock.Object);
		_unitOfWorkMock.Setup(u => u.StudentRepository).Returns(_studentRepositoryMock.Object);

		_handler = new RegisterCommandHandler(_unitOfWorkMock.Object, _securityServiceMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ShouldThrowDuplicateEmailException_WhenEmailAlreadyExists()
	{
		var command = new RegisterCommand { Email = "existing@test.com" };
		_accountRepositoryMock.Setup(r => r.GetByUsernameAsync(command.Email)).ReturnsAsync(new Account());

		Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

		await act.Should().ThrowAsync<DuplicateEmailException>();
	}

	[Fact]
	public async Task Handle_ShouldCreateStudentAndAccount_WhenRegistrationIsSuccessful()
	{
		var command = new RegisterCommand { FirstName = "Daniel", LastName = "Ramirez", Email = "test@test.com", Password = "password123" };

		_accountRepositoryMock.Setup(r => r.GetByUsernameAsync(command.Email)).ReturnsAsync((Account)null);
		_accountRepositoryMock.Setup(r => r.CountSimilarUsernamesAsync("dramirez")).ReturnsAsync(0);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().NotBeEmpty();
		_studentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Once);
		_accountRepositoryMock.Verify(r => r.AddAsync(It.Is<Account>(a => a.Username == "dramirez")), Times.Once);
		_unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
	}
}