using Bogus;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Enums;
using StudentRegistration.Infraestructure.Persistence;

namespace StudentRegistration.Infraestructure
{
	public static class DataSeeder
	{
		public static void Seed(AppDbContext context, ISecurityService securityService)
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			var professorFaker = new Faker<Professor>("es")
				.RuleFor(p => p.FirstName, f => f.Name.FirstName())
				.RuleFor(p => p.LastName, f => f.Name.LastName());

			var professors = professorFaker.Generate(5);
			context.Professors.AddRange(professors);
			context.SaveChanges();

			var courseFaker = new Faker<Course>("es")
				.RuleFor(c => c.Name, f => $"{Capitalize(f.Hacker.IngVerb())} de {Capitalize(f.Hacker.Noun())}")
				.RuleFor(c => c.Credits, 3);

			var allCourses = new List<Course>();

			foreach (var professor in professors)
			{
				var coursesForProfessor = courseFaker.Generate(2);
				foreach (var course in coursesForProfessor)
				{
					course.ProfessorId = professor.Id;
				}

				allCourses.AddRange(coursesForProfessor);
			}

			context.Courses.AddRange(allCourses);

			securityService.CreatePasswordHash("123456", out byte[] passwordHash, out byte[] passwordSalt);

			var student = new Student
			{
				FirstName = "Usuario",
				LastName = "Prueba",
				Email = "test@test.com"
			};

			context.Students.Add(student);
			context.SaveChanges();

			var account = new Account
			{
				Email = "test@test.com",
				Username = "test",
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				IdReferencia = student.Id,
				TipoCuenta = EnumTipoCuenta.Estudiante
			};

			context.Account.Add(account);

			context.SaveChanges();
		}

		private static string Capitalize(string input)
		{
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			return char.ToUpper(input[0]) + input.Substring(1);
		}
	}
}