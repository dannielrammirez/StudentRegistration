using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Features;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.UnitTests.Features.Enrollments;

public class CreateEnrollmentCommandHandlerTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<ILogger<CreateEnrollmentCommandHandler>> _loggerMock;
	private readonly Mock<IStudentRepository> _studentRepositoryMock;
	private readonly Mock<ICourseRepository> _courseRepositoryMock;
	private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
	private readonly CreateEnrollmentCommandHandler _handler;

	public CreateEnrollmentCommandHandlerTests()
	{
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_loggerMock = new Mock<ILogger<CreateEnrollmentCommandHandler>>();
		_studentRepositoryMock = new Mock<IStudentRepository>();
		_courseRepositoryMock = new Mock<ICourseRepository>();
		_enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();

		_unitOfWorkMock.Setup(u => u.StudentRepository).Returns(_studentRepositoryMock.Object);
		_unitOfWorkMock.Setup(u => u.CourseRepository).Returns(_courseRepositoryMock.Object);
		_unitOfWorkMock.Setup(u => u.EnrollmentRepository).Returns(_enrollmentRepositoryMock.Object);

		_handler = new CreateEnrollmentCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
	}

	[Fact]
	public async Task Handle_ShouldThrowException_WhenStudentExceedsCourseLimit()
	{
		var studentId = Guid.NewGuid();
		var command = new CreateEnrollmentCommand { AuthenticatedAccountId = studentId, CourseIds = new List<Guid> { Guid.NewGuid() } };
		var existingEnrollments = new List<Enrollment> { new(), new(), new() };

		_studentRepositoryMock.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(new Student());
		_enrollmentRepositoryMock.Setup(r => r.GetEnrollmentsByStudentIdAsync(studentId)).ReturnsAsync(existingEnrollments);

		Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

		await act.Should().ThrowAsync<Exception>().WithMessage("La selección excede el límite de 3 materias.");
	}

	[Fact]
	public async Task Handle_ShouldSucceed_WhenEnrollmentIsValid()
	{
		var studentId = Guid.NewGuid();
		var course1 = new Course { Id = Guid.NewGuid(), ProfessorId = Guid.NewGuid() };
		var command = new CreateEnrollmentCommand { AuthenticatedAccountId = studentId, CourseIds = new List<Guid> { course1.Id } };

		_studentRepositoryMock.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(new Student());
		_enrollmentRepositoryMock.Setup(r => r.GetEnrollmentsByStudentIdAsync(studentId)).ReturnsAsync(new List<Enrollment>());
		_courseRepositoryMock.Setup(r => r.GetByIdAsync(course1.Id)).ReturnsAsync(course1);

		await _handler.Handle(command, CancellationToken.None);

		_enrollmentRepositoryMock.Verify(r => r.AddRangeAsync(It.Is<List<Enrollment>>(l => l.Count == 1)), Times.Once);
		_unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
	}
}