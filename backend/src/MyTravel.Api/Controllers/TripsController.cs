using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;                                                                                                                                                                                                                               
using MyTravel.Api.Extensions;                                                                                                                         
using MyTravel.Application.DTOs.Trips;                                                                                                                 
using MyTravel.Application.Interfaces;

namespace MyTravel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController(ITripService tripServ) : ControllerBase
    {
        
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status201Created)]                                                                      
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTrip([FromBody]CreateTripDto dto)
        {
            var userId = User.GetUserId();
            var trip = await tripServ.CreateTripAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip);// Tengo que crear los metodos 
        }

        [HttpGet]                                                                                                                                          
        [Authorize]                                                                                                                                        
        [ProducesResponseType(typeof(IEnumerable<TripResponseDto>), StatusCodes.Status200OK)]                                                              
        public async Task<IActionResult> GetMyTrips()
        {
            var userId = User.GetUserId();
            var trips = await tripServ.GetUserTripAsync(userId); 
            return Ok(trips);
        }

        [HttpGet("public")]
        [AllowAnonymous]                                                                                                                                   
        [ProducesResponseType(typeof(IEnumerable<TripResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPublicTrips()
        {
            var trips = await tripServ.GetPublicTripsAsync();
            return Ok(trips);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]                                                                                                                                   
        [ProducesResponseType(typeof(TripDetailResponseDto), StatusCodes.Status200OK)]                                                                     
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            Guid? userId = null;
            if (User.Identity?.IsAuthenticated == true)
            {
                userId = User.GetUserId();
            }
            var trip = await tripServ.GetTripByIdAsync(id, userId);
            if (trip == null)
            {
                return NotFound(new { message = "El viaje no existe o no tienes permisos para visualizarlo." });
            }
            return Ok(trip);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]                                                                           
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTrip(Guid id, [FromBody] UpdateTripDto dto)
        {
            var userId = User.GetUserId();
            var updatedTrip = await tripServ.UpdateTripAsync(id, userId, dto);
            if (updatedTrip == null)
            {
                return NotFound(new { message = "El viaje no existe o no tienes permisos para actualizarlo." });
            }
            return Ok(updatedTrip);
        }
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var userId = User.GetUserId();
            var deleted = await tripServ.DeleteTripAsync(id, userId);
            if (!deleted)
            {
                return NotFound(new { message = "El viaje no existe o no tienes permisos para eliminarlo." });
            }
            return NoContent();
        }


       
    }
}
