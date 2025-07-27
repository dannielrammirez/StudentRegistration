using Moq;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Features;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Exceptions;
using FluentAssertions;

namespace StudentRegistration.Application.UnitTests
{
	public class CreateEnrollmentCommandHandlerTests
	{
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;
		private readonly CreateEnrollmentCommandHandler _handler;

		public CreateEnrollmentCommandHandlerTests()
		{
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_handler = new CreateEnrollmentCommandHandler(_unitOfWorkMock.Object);
		}

		[Fact]
		public async Task Handle_ShouldThrowException_WhenStudentHasThreeOrMoreCourses()
		{
			// Arrange (Preparar)
			var command = new CreateEnrollmentCommand { StudentId = Guid.NewGuid(), CourseId = Guid.NewGuid() };

			var student = new Student { Id = command.StudentId };
			var existingEnrollments = new List<Enrollment>
		{
			new Enrollment { Course = new Course { Professor = new Professor() } },
			new Enrollment { Course = new Course { Professor = new Professor() } },
			new Enrollment { Course = new Course { Professor = new Professor() } }
		};

			_unitOfWorkMock.Setup(u => u.StudentRepository.GetByIdAsync(command.StudentId)).ReturnsAsync(student);
			_unitOfWorkMock.Setup(u => u.CourseRepository.GetByIdAsync(command.CourseId)).ReturnsAsync(new Course());
			_unitOfWorkMock.Setup(u => u.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(command.StudentId)).ReturnsAsync(existingEnrollments);

			// Act (Actuar)
			Func<Task> act = async () => await _handler.Handle(command, default);

			// Assert (Verificar)
			await act.Should().ThrowAsync<EnrollmentRuleException>()
				.WithMessage("Límite de 3 materias alcanzado.");
		}

		[Fact]
		public async Task Handle_ShouldThrowException_WhenStudentAlreadyHasCourseWithSameProfessor()
		{
			// Arrange
			var professorId = Guid.NewGuid();
			var command = new CreateEnrollmentCommand { StudentId = Guid.NewGuid(), CourseId = Guid.NewGuid() };

			var student = new Student { Id = command.StudentId };
			var courseToEnroll = new Course { Id = command.CourseId, ProfessorId = professorId };
			var existingEnrollments = new List<Enrollment>
		{
			new Enrollment { Course = new Course { ProfessorId = professorId } }
		};

			_unitOfWorkMock.Setup(u => u.StudentRepository.GetByIdAsync(command.StudentId)).ReturnsAsync(student);
			_unitOfWorkMock.Setup(u => u.CourseRepository.GetByIdAsync(command.CourseId)).ReturnsAsync(courseToEnroll);
			_unitOfWorkMock.Setup(u => u.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(command.StudentId)).ReturnsAsync(existingEnrollments);

			// Act
			Func<Task> act = async () => await _handler.Handle(command, default);

			// Assert
			await act.Should().ThrowAsync<EnrollmentRuleException>()
				.WithMessage("Ya tienes una materia con este profesor.");
		}

		[Fact]
		public async Task Handle_ShouldAddEnrollmentAndSaveChanges_WhenValidationSucceeds()
		{
			// Arrange
			var command = new CreateEnrollmentCommand { StudentId = Guid.NewGuid(), CourseId = Guid.NewGuid() };

			_unitOfWorkMock.Setup(u => u.StudentRepository.GetByIdAsync(command.StudentId)).ReturnsAsync(new Student());
			_unitOfWorkMock.Setup(u => u.CourseRepository.GetByIdAsync(command.CourseId)).ReturnsAsync(new Course { Professor = new Professor() });
			_unitOfWorkMock.Setup(u => u.EnrollmentRepository.GetEnrollmentsByStudentIdAsync(command.StudentId)).ReturnsAsync(new List<Enrollment>());

			// Act
			await _handler.Handle(command, default);

			// Assert
			_unitOfWorkMock.Verify(u => u.EnrollmentRepository.AddAsync(It.IsAny<Enrollment>()), Times.Once);
			_unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
		}
	}
}