using StudentRegistration.Application.Common;
using StudentRegistration.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace StudentRegistration.Presentation.API.Middleware;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;
	private readonly IHostEnvironment _env;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
	{
		_next = next;
		_logger = logger;
		_env = env;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
			context.Response.ContentType = "application/json";

			var statusCode = HttpStatusCode.InternalServerError;
			object response;

			switch (ex)
			{
				case DuplicateEmailException:
					statusCode = HttpStatusCode.Conflict;
					response = new GenericResponse<object>(null, false, ex.Message);
					break;
				default:
					response = _env.IsDevelopment()
						? new GenericResponse<object>(ex.StackTrace, false, ex.Message)
						: new GenericResponse<object>(null, false, "Ha ocurrido un error interno en el servidor.");
					break;
			}

			context.Response.StatusCode = (int)statusCode;

			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
			var json = JsonSerializer.Serialize(response, options);

			await context.Response.WriteAsync(json);
		}
	}
}