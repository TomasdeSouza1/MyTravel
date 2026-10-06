using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Api.Extensions;
using MyTravel.Application.DTOs.Trips;
using MyTravel.Application.Interfaces.Trip;

namespace MyTravel.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController(ITripService tripService) : ControllerBase
{
    private readonly ITripService _tripService = tripService;

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto dto)
    {
        var userId = User.GetUserId();
        var trip = await _tripService.CreateTripAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<TripResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyTrips()
    {
        var userId = User.GetUserId();
        var trips = await _tripService.GetUserTripAsync(userId);
        return Ok(trips);
    }

    [HttpGet("public")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<TripResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublicTrips()
    {
        var trips = await _tripService.GetPublicTripsAsync();
        return Ok(trips);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TripDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        Guid? userId = null;
        if (User.Identity?.IsAuthenticated == true)
        {
            userId = User.GetUserId();
        }
        var trip = await _tripService.GetTripByIdAsync(id, userId);
        if (trip == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El viaje no existe o no tienes permisos para visualizarlo.");
        }
        return Ok(trip);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTrip(Guid id, [FromBody] UpdateTripDto dto)
    {
        var userId = User.GetUserId();
        var updatedTrip = await _tripService.UpdateTripAsync(id, userId, dto);
        if (updatedTrip == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El viaje no existe o no tienes permisos para actualizarlo.");
        }
        return Ok(updatedTrip);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTrip(Guid id)
    {
        var userId = User.GetUserId();
        var deleted = await _tripService.DeleteTripAsync(id, userId);
        if (!deleted)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El viaje no existe o no tienes permisos para eliminarlo.");
        }
        return NoContent();
    }
}
