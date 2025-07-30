using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure.Repositories
{
	public class ProfessorRepository : IProfessorRepository
	{
		private readonly AppDbContext _context;

		public ProfessorRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Professor> GetByIdAsync(Guid id)
		{
			var response = await _context.Professors.FindAsync(id);
			return response;
		}
	}
}