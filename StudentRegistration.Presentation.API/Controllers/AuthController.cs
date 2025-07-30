using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Application.Features.Auth.Commands;
using System.Security.Claims;

namespace StudentRegistration.Presentation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Registra un nuevo estudiante y su cuenta de autenticación en el sistema.
		/// </summary>
		/// <param name="request">Objeto DTO con los datos para el registro: nombre, apellido, email y contraseña.</param>
		/// <returns>Un objeto JSON con el ID del nuevo estudiante creado.</returns>
		/// <response code="200">Retorna el ID del estudiante recién creado.</response>
		/// <response code="400">Si los datos de entrada no son válidos o el email ya está en uso.</response>
		[HttpPost("register")]
		[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Register(RegisterRequestDto request)
		{
			var command = new RegisterCommand
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				Password = request.Password
			};

			var studentId = await _mediator.Send(command);

			return Ok(new { Id = studentId });
		}

		/// <summary>
		/// Genera un nuevo par de tokens usando un Refresh Token válido.
		/// </summary>
		/// <param name="request">DTO que contiene el AccessToken expirado y el RefreshToken.</param>
		/// <returns>Un nuevo LoginResponseDto con los nuevos tokens.</returns>
		[HttpPost("refresh-token")]
		[ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
		{
			var command = new RefreshTokenCommand
			{
				AccessToken = request.AccessToken,
				RefreshToken = request.RefreshToken
			};
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		/// <summary>
		/// Revoca todos los Refresh Tokens para el usuario autenticado.
		/// </summary>
		/// <returns>Respuesta exitosa sin contenido.</returns>
		[Authorize]
		[HttpPost("revoke")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Revoke()
		{
			var accountId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var command = new RevokeTokenCommand { AccountId = accountId };
			await _mediator.Send(command);
			return NoContent();
		}

		/// <summary>
		/// Autentica a un estudiante y devuelve los tokens de acceso y refresco.
		/// </summary>
		/// <param name="request">Credenciales de inicio de sesión (email o username) y contraseña.</param>
		/// <returns>Un objeto con el AccessToken y el RefreshToken.</returns>
		/// <response code="200">Retorna los tokens si las credenciales son correctas.</response>
		/// <response code="401">Si las credenciales son incorrectas.</response>
		[HttpPost("login")]
		[ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Login(LoginRequestDto request)
		{
			var command = new LoginCommand
			{
				LoginIdentifier = request.LoginIdentifier,
				Password = request.Password
			};

			var result = await _mediator.Send(command);

			return Ok(result);
		}
	}
}