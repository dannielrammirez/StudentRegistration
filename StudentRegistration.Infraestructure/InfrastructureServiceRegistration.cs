using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Infraestructure.Persistence;
using StudentRegistration.Infraestructure.Repositories;
using StudentRegistration.Infrastructure.Services;

namespace StudentRegistration.Infrastructure;

public static class InfrastructureServiceRegistration
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<ISecurityService, SecurityService>();

		return services;
	}
}