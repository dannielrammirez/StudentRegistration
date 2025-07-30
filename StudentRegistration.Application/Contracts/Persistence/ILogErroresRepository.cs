using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Contracts.Persistence
{
	public interface ILogErroresRepository
	{
		bool InsertErrores(List<LogErrores> listErrores);
		bool InsertError(LogErrores objLogError);
		Task<bool> InsertErrorAsync(LogErrores objLogError);
	}
}