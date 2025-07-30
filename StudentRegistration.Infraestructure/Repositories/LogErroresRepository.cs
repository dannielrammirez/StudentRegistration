using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class LogErroresRepository : ILogErroresRepository
	{
		private readonly AppDbContext _context;

		public LogErroresRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<bool> InsertErrorAsync(LogErrores objLogError)
		{
			//var response = await _context.AddAsync(objLogError);
			//return response;
			throw new NotImplementedException();
		}

		public bool InsertErrores(List<LogErrores> listErrores)
		{
			throw new NotImplementedException();
		}

		bool ILogErroresRepository.InsertError(LogErrores objLogError)
		{
			throw new NotImplementedException();
		}
	}
}