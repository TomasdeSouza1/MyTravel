using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Api.Extensions;
using MyTravel.Application.DTOs.ItineraryDays;
using MyTravel.Application.Interfaces.Itinerary;

namespace MyTravel.Api.Controllers;

[ApiController]
public class ItineraryDaysController(IItineraryDayService dayService) : ControllerBase
{
    private readonly IItineraryDayService _dayService = dayService;

    [HttpPost("api/trips/{tripId:guid}/days")]
    [Authorize]
    [ProducesResponseType(typeof(ItineraryDayResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddDay(Guid tripId, [FromBody] CreateItineraryDayDto dto)
    {
        var userId = User.GetUserId();
        var day = await _dayService.AddDayAsync(tripId, userId, dto);

        if (day == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El viaje no existe o no tienes permisos para agregar días.");
        }

        return StatusCode(StatusCodes.Status201Created, day);
    }

    [HttpPut("api/days/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ItineraryDayResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDay(Guid id, [FromBody] UpdateItineraryDayDto dto)
    {
        var userId = User.GetUserId();
        var updated = await _dayService.UpdateDayAsync(id, userId, dto);

        if (updated == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El día no existe o no tienes permisos para actualizarlo.");
        }

        return Ok(updated);
    }

    [HttpDelete("api/days/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDay(Guid id)
    {
        var userId = User.GetUserId();
        var deleted = await _dayService.DeleteDayAsync(id, userId);

        if (!deleted)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El día no existe o no tienes permisos para eliminarlo.");
        }

        return NoContent();
    }
}
