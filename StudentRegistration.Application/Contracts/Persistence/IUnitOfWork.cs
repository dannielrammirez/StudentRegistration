namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface IUnitOfWork : IDisposable
	{
		IStudentRepository StudentRepository { get; }
		ICourseRepository CourseRepository { get; }
		IProfessorRepository ProfessorRepository { get; }
		IEnrollmentRepository EnrollmentRepository { get; }
		IRefreshTokenRepository RefreshTokenRepository { get; }
		IAccountRepository AccountRepository { get; }
		Task<int> SaveChangesAsync();
	}
}