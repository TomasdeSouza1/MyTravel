using System;
using MyTravel.Application.DTOs.Trips;

namespace MyTravel.Application.Interfaces;

public interface ITripService
{
    Task<TripResponseDto> CreateTripAsync(Guid userId, CreateTripDto dto);
    Task<IEnumerable<TripResponseDto>> GetUserTripAsync(Guid userId);
    Task<IEnumerable<TripResponseDto>> GetPublicTripsAsync();
    Task<TripDetailResponseDto?> GetTripByIdAsync(Guid tripId, Guid? userId); 
    Task<TripResponseDto?> UpdateTripAsync(Guid tripId, Guid userId, UpdateTripDto dto);
    Task<bool> DeleteTripAsync(Guid tripId, Guid userId);
}
