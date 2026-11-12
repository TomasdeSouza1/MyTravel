using System;
using MyTravel.Application.DTOs.Collaboration;
using MyTravel.Application.DTOs.Trips;

namespace MyTravel.Application.Interfaces.Collaboration;

public interface ICollaborationService
{
    Task<TripResponseDto?> JoinTripAsync(Guid userId, JointripDto joinTripDto); 
    Task<IEnumerable<TripMemberDto>?> GetTripMemberAsync(Guid tripId, Guid userId);
    Task<bool> RemoveTripMemberAsync(Guid tripId, Guid userId, Guid CurrentUserId);
    Task<string?> RegenerateInviteTokenAsync(Guid tripId, Guid userId);
}
