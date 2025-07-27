using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Application.Mappers;
using StudentRegistration.Infraestructure.Persistence;
using StudentRegistration.Infraestructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Agregar el DbContext para Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new MappingProfile()) });

builder.Services.AddControllers();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "StudentRegistration API", Version = "v1" });
});

var app = builder.Build();

// Habilitar Swagger solo en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentRegistration API V1");
		c.RoutePrefix = string.Empty; // Para que Swagger aparezca en la raíz del sitio
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();