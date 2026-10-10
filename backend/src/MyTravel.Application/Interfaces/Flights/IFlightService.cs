using System;
using MyTravel.Application.DTOs.Flights;

namespace MyTravel.Application.Interfaces.Flights;

public interface IFlightService
{
    Task<IEnumerable<FlightResponseDto>?> GetFlightsByTripIdAsync(Guid tripId, Guid userId);                                                            
    Task<FlightResponseDto?> GetFlightByIdAsync(Guid flightId, Guid userId);                                                                           
    Task<FlightResponseDto?> AddFlightAsync(Guid tripId, Guid userId, CreateFlightDto dto);                                                            
    Task<FlightResponseDto?> UpdateFlightAsync(Guid flightId, Guid userId, UpdateFlightDto dto);                                                       
    Task<bool> DeleteFlightAsync(Guid flightId, Guid userId);
}
