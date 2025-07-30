using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentRegistration.Application;
using StudentRegistration.Application.Contracts.Persistence;
using StudentRegistration.Infraestructure;
using StudentRegistration.Infraestructure.Persistence;
using StudentRegistration.Infrastructure;
using StudentRegistration.Presentation.API.Middleware;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp",
		policy =>
		{
			policy.WithOrigins("http://localhost:4200")
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		var tokenKey = builder.Configuration.GetSection("Jwt:Key").Value;
		if (string.IsNullOrEmpty(tokenKey))
			throw new Exception("Token key not configured in appsettings.json");

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Registration API", Version = "v1" });

	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	c.IncludeXmlComments(xmlPath);

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Autenticación JWT. Ejemplo: \"Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
			},
			Array.Empty<string>()
		}
	});
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		var dbContext = services.GetRequiredService<AppDbContext>();
		var securityService = services.GetRequiredService<ISecurityService>();
		DataSeeder.Seed(dbContext, securityService);
	}

	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/Auth/swagger.json", "Auth API");
		c.SwaggerEndpoint("/swagger/Courses/swagger.json", "Courses API");
		c.SwaggerEndpoint("/swagger/Enrollments/swagger.json", "Enrollments API");
		c.SwaggerEndpoint("/swagger/HomeController/swagger.json", "Home API");
		c.SwaggerEndpoint("/swagger/StudentRegistration/swagger.json", "StudentRegistration API");
		c.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();