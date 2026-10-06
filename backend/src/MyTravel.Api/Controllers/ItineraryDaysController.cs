using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Api.Extensions;
using MyTravel.Application.DTOs.ItineraryDays;
using MyTravel.Application.Interfaces;

namespace MyTravel.Api.Controllers;

[ApiController]
public class ItineraryDaysController(IItineraryDayService dayService) : ControllerBase
{
    [HttpPost("api/trips/{tripId:guid}/days")]
    [Authorize]
    [ProducesResponseType(typeof(ItineraryDayResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddDay(Guid tripId, [FromBody] CreateItineraryDayDto dto)
    {
        var userId = User.GetUserId();
        var day = await dayService.AddDayAsync(tripId, userId, dto);

        if (day == null)
        {
            return NotFound(new { message = "El viaje no existe o no tienes permisos para agregar días." });
        }

        return StatusCode(StatusCodes.Status201Created, day);
    }

    [HttpPut("api/days/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ItineraryDayResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDay(Guid id, [FromBody] UpdateItineraryDayDto dto)
    {
        var userId = User.GetUserId();
        var updated = await dayService.UpdateDayAsync(id, userId, dto);

        if (updated == null)
        {
            return NotFound(new { message = "El día no existe o no tienes permisos para actualizarlo." });
        }

        return Ok(updated);
    }

    [HttpDelete("api/days/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDay(Guid id)
    {
        var userId = User.GetUserId();
        var deleted = await dayService.DeleteDayAsync(id, userId);

        if (!deleted)
        {
            return NotFound(new { message = "El día no existe o no tienes permisos para eliminarlo." });
        }

        return NoContent();
    }
}
