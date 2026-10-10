using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Api.Extensions;
using MyTravel.Application.DTOs.Flights;
using MyTravel.Application.Interfaces.Flights;

namespace MyTravel.Api.Controllers;

[ApiController]
public class FlightsController(IFlightService flightService) : ControllerBase
{
    [HttpGet("api/trips/{tripId:guid}/flights")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<FlightResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlightsByTripId(Guid tripId)
    {
        var userId = User.GetUserId();
        var flights = await flightService.GetFlightsByTripIdAsync(tripId, userId);

        if (flights == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El viaje no existe o no tienes permiso para verlo.");
        }

        return Ok(flights);
    }

    [HttpGet("api/flights/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(FlightResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFlightById(Guid id)
    {
        var userId = User.GetUserId();
        var flight = await flightService.GetFlightByIdAsync(id, userId);

        if (flight == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El vuelo no existe o no tienes permiso para verlo.");
        }

        return Ok(flight);
    }

    [HttpPost("api/trips/{tripId:guid}/flights")]
    [Authorize]
    [ProducesResponseType(typeof(FlightResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddFlight(Guid tripId, [FromBody] CreateFlightDto dto)
    {
        var userId = User.GetUserId();
        var flight = await flightService.AddFlightAsync(tripId, userId, dto);

        if (flight == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El viaje no existe o no tienes permiso para agregar vuelos.");
        }

        return StatusCode(StatusCodes.Status201Created, flight);
    }

    [HttpPut("api/flights/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(FlightResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFlight(Guid id, [FromBody] UpdateFlightDto dto)
    {
        var userId = User.GetUserId();
        var updatedFlight = await flightService.UpdateFlightAsync(id, userId, dto);

        if (updatedFlight == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El vuelo no existe o no tienes permiso para actualizarlo.");
        }

        return Ok(updatedFlight);
    }

    [HttpDelete("api/flights/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFlight(Guid id)
    {
        var userId = User.GetUserId();
        var deleted = await flightService.DeleteFlightAsync(id, userId);

        if (!deleted)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "No encontrado",
                detail: "El vuelo no existe o no tienes permiso para eliminarlo.");
        }

        return NoContent();
    }
}
