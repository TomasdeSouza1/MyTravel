using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Api.Extensions;
using MyTravel.Application.DTOs.Activities;
using MyTravel.Application.Interfaces.Activity;

namespace MyTravel.Api.Controllers;

[ApiController]
public class ActivitiesController(IActivityService activityService) : ControllerBase
{
    private readonly IActivityService _activityService = activityService;

    [HttpPost("api/days/{dayId:guid}/activities")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddActivity(Guid dayId, [FromBody] CreateActivityDto dto)
    {
        var userId = User.GetUserId();
        var activity = await _activityService.AddActivityAsync(dayId, userId, dto);

        if (activity == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El día no existe o no tienes permisos para agregar actividades.");
        }

        return StatusCode(StatusCodes.Status201Created, activity);
    }

    [HttpPut("api/activities/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateActivity(Guid id, [FromBody] UpdateActivityDto dto)
    {
        var userId = User.GetUserId();
        var updated = await _activityService.UpdateActivityAsync(id, userId, dto);

        if (updated == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "La actividad no existe o no tienes permisos para actualizarla.");
        }

        return Ok(updated);
    }

    [HttpDelete("api/activities/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        var userId = User.GetUserId();
        var deleted = await _activityService.DeleteActivityAsync(id, userId);

        if (!deleted)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "La actividad no existe o no tienes permisos para eliminarla.");
        }

        return NoContent();
    }

    [HttpPut("api/days/{dayId:guid}/activities/reorder")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReorderActivities(Guid dayId, [FromBody] ReorderActivitiesDto dto)
    {
        var userId = User.GetUserId();
        var success = await _activityService.ReorderActivitiesAsync(dayId, userId, dto);

        if (!success)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El día no existe o no tienes permisos para reordenar sus actividades.");
        }

        return NoContent();
    }
}
