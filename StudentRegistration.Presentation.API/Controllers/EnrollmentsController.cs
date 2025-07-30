using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Application.Features;
using System.Security.Claims;

namespace StudentRegistration.Presentation.API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class EnrollmentsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public EnrollmentsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Inscribe a un estudiante en un conjunto de materias.
		/// </summary>
		/// <remarks>
		/// Este endpoint aplica las reglas de negocio: máximo 3 materias y no se puede repetir profesor.
		/// </remarks>
		/// <param name="request">DTO que contiene el ID del estudiante y la lista de IDs de las materias.</param>
		/// <returns>Respuesta exitosa si la inscripción es válida.</returns>
		/// <response code="200">La inscripción se procesó exitosamente.</response>
		/// <response code="400">Si la solicitud no es válida o se viola una regla de negocio.</response>
		/// <response code="401">Si el usuario no está autenticado.</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> CreateEnrollment(EnrollmentRequestDto request)
		{
			var accountId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var command = new CreateEnrollmentCommand
			{
				AuthenticatedAccountId = accountId,
				CourseIds = request.CourseIds
			};

			await _mediator.Send(command);

			return Ok();
		}
	}
}