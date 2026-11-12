using System;
using Microsoft.EntityFrameworkCore;
using MyTravel.Application.DTOs.Collaboration;
using MyTravel.Application.DTOs.Trips;
using MyTravel.Application.Interfaces.Collaboration;
using MyTravel.Domain.Entities;
using MyTravel.Domain.Enums;
using MyTravel.Infrastructure.Persistence;

namespace MyTravel.Infrastructure.Services.Collaboration;

public class CollaborationService(ApplicationDbContext context) : ICollaborationService
{
    private const int MaxMembersPerTrip = 10;

    public async Task<TripResponseDto?> JoinTripAsync(Guid userId, JointripDto jointripDto)
    {
        var trip = await context.Trips
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.InviteToken == jointripDto.InviteToken);

        if (trip == null) return null;

        var alredyMember = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId);
        if (!alredyMember)
        {
            if (trip.Members.Count >= MaxMembersPerTrip) return null;

            context.TripMembers.Add(new TripMember
            {
                TripId = trip.Id,
                UserId = userId,
                Role = MemberRole.Editor
            });
            await context.SaveChangesAsync();
        }

        return new TripResponseDto
        {
            Id = trip.Id,
            UserId = trip.UserId,
            Title = trip.Title,
            Description = trip.Description,
            DestinationCountry = trip.DestinationCountry,
            DestinationCity = trip.DestinationCity,
            CoverImageUrl = trip.CoverImageUrl,
            BaseCurrency = trip.BaseCurrency,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate,
            TotalBudget = trip.TotalBudget,
            InviteToken = string.Empty,
            IsPublic = trip.IsPublic,
            CreatedAt = trip.CreatedAt
        };
    }

    public async Task<IEnumerable<TripMemberDto>?> GetTripMemberAsync(Guid tripId, Guid userId)
    {
        var trip = await context.Trips
            .AsNoTracking()
            .Include(t => t.Members)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return null;

        var isMember = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId);
        if (!isMember) return null;

        return trip.Members
            .OrderBy(m => m.Role)
            .Select(m => new TripMemberDto
            {
                TripId = m.TripId,
                UserId = m.UserId,
                FullName = m.User?.FullName ?? "",
                Email = m.User?.Email ?? "",
                Role = m.Role
            })
            .ToList();
    }

    public async Task<bool> RemoveTripMemberAsync(Guid tripId, Guid memberUserId, Guid currentUserId)
    {
        var trip = await context.Trips
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return false;

        //check if the user is the owner of the trip 
        //the owner of the trip cannot be removed from the trip
        if (trip.UserId == memberUserId) return false;

        var canRemoveMember = trip.UserId == currentUserId || currentUserId == memberUserId;
        if (!canRemoveMember) return false;

        var Member = trip.Members.FirstOrDefault(m => m.UserId == memberUserId);
        if (Member == null) return false;
        
        trip.Members.Remove(Member);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> RegenerateInviteTokenAsync(Guid tripId, Guid userId)
    {
        var trip = await context.Trips
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null || trip.UserId != userId) return null;

        trip.RegenerateInviteToken();
        await context.SaveChangesAsync();
        return trip.InviteToken;
    }
}
