using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using MyTravel.Api.Extensions;
using MyTravel.Application.Interfaces.Collaboration;
using MyTravel.Application.DTOs.Collaboration;
using MyTravel.Application.DTOs.Trips;

namespace MyTravel.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class CollaborationController(ICollaborationService collaborationService) : ControllerBase
    {
        [HttpPost("api/trips/join")]
        [EnableRateLimiting("strict-by-ip")]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> JoinTrip([FromBody] JointripDto Dto)
        {
            var userId = User.GetUserId();
            var trip = await collaborationService.JoinTripAsync(userId, Dto);
            if (trip == null)
            {
                return Problem(
                    title: "No encontrado",
                    detail: "El token de invitacion proporcionado no es válido o ha expirado.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            return Ok(trip);
        }

        [HttpGet("api/trips/{tripId:guid}/members")]
        [ProducesResponseType(typeof(IEnumerable<TripMemberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMembers(Guid tripId)
        {
            var UserId = User.GetUserId();
            var members = await collaborationService.GetTripMemberAsync(tripId, UserId);
            if (members == null)
            {
                return Problem(
                    title: "No encontrado",
                    detail: "El viaje no existe o no tienes permiso para acceder a sus miembros.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            return Ok(members);
        }

        [HttpDelete("api/trips/{tripId:guid}/members/{memberUserId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveMember(Guid tripId, Guid memberUserId)
        {
            var currentUserId = User.GetUserId();
            var removed = await collaborationService.RemoveTripMemberAsync(tripId, memberUserId, currentUserId);
            if (!removed)
            {
                return Problem(
                    title: "No encontrado",
                    detail: "El viaje o el miembro no existen, o no tienes permiso para eliminar a este miembro.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }
            return NoContent();
        }

        [HttpPost("api/trips/{tripId:guid}/invite-token")]
        [EnableRateLimiting("strict-by-ip")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateInviteToken(Guid tripId)
        {
            var userId = User.GetUserId();
            var newtoken = await collaborationService.RegenerateInviteTokenAsync(tripId, userId);
            if (newtoken == null)
            {
                return Problem(
                    title: "No encontrado",
                    detail: "El viaje no existe o no tienes permiso para generar un token de invitación.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            return Ok(new { InviteToken = newtoken });
        }
    }
}
