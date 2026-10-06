using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Api.Extensions;
using MyTravel.Application.DTOs.Activities;
using MyTravel.Application.Interfaces;

namespace MyTravel.Api.Controllers;

[ApiController]
public class ActivitiesController(IActivityService activityService) : ControllerBase
{
    [HttpPost("api/days/{dayId:guid}/activities")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddActivity(Guid dayId, [FromBody] CreateActivityDto dto)
    {
        var userId = User.GetUserId();
        var activity = await activityService.AddActivityAsync(dayId, userId, dto);

        if (activity == null)
        {
            return NotFound(new { message = "El día no existe o no tienes permisos para agregar actividades." });
        }

        return StatusCode(StatusCodes.Status201Created, activity);
    }

    [HttpPut("api/activities/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateActivity(Guid id, [FromBody] UpdateActivityDto dto)
    {
        var userId = User.GetUserId();
        var updated = await activityService.UpdateActivityAsync(id, userId, dto);

        if (updated == null)
        {
            return NotFound(new { message = "La actividad no existe o no tienes permisos para actualizarla." });
        }

        return Ok(updated);
    }

    [HttpDelete("api/activities/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        var userId = User.GetUserId();
        var deleted = await activityService.DeleteActivityAsync(id, userId);

        if (!deleted)
        {
            return NotFound(new { message = "La actividad no existe o no tienes permisos para eliminarla." });
        }

        return NoContent();
    }

    [HttpPut("api/days/{dayId:guid}/activities/reorder")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReorderActivities(Guid dayId, [FromBody] ReorderActivitiesDto dto)
    {
        var userId = User.GetUserId();
        var success = await activityService.ReorderActivitiesAsync(dayId, userId, dto);

        if (!success)
        {
            return NotFound(new { message = "El día no existe o no tienes permisos para reordenar sus actividades." });
        }

        return NoContent();
    }
}
