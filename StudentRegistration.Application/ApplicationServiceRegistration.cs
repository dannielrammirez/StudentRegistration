using Microsoft.Extensions.DependencyInjection;
using StudentRegistration.Application.Mappers;
using System.Reflection;

namespace StudentRegistration.Application
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddAutoMapper(cfg => { cfg.AddProfile(new MappingProfile()); });
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			return services;
		}
	}
}