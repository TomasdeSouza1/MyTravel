using Microsoft.EntityFrameworkCore;
using MyTravel.Application.DTOs.Activities;
using MyTravel.Application.Interfaces.Activity;
using MyTravel.Domain.Entities;
using MyTravel.Domain.Enums;
using MyTravel.Infrastructure.Persistence;

namespace MyTravel.Infrastructure.Services.Activities;

public class ActivityService(ApplicationDbContext context) : IActivityService
{
    public async Task<ActivityResponseDto?> AddActivityAsync(Guid dayId, Guid userId, CreateActivityDto dto)
    {
        // Consulta unificada en 1 solo viaje a la BD gracias a las propiedades de navegación
        var day = await context.ItineraryDays
            .Include(d => d.Trip)
                .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(d => d.Id == dayId);

        if (day?.Trip == null) return null;

        var trip = day.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return null;

        var nextOrderIndex = dto.OrderIndex;
        if (nextOrderIndex == 0)
        {
            var maxIndex = await context.Activities
                .Where(a => a.ItineraryDayId == dayId)
                .Select(a => (int?)a.OrderIndex)
                .MaxAsync() ?? -1;
            nextOrderIndex = maxIndex + 1;
        }

        var activity = new Activity
        {
            ItineraryDayId = dayId,
            Name = dto.Name,
            Category = dto.Category,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Address = dto.Address,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            OrderIndex = nextOrderIndex,
            BookingReference = dto.BookingReference,
            Notes = dto.Notes
        };

        context.Activities.Add(activity);
        await context.SaveChangesAsync();

        return MapToResponseDto(activity);
    }

    public async Task<ActivityResponseDto?> UpdateActivityAsync(Guid activityId, Guid userId, UpdateActivityDto dto)
    {
       
        var activity = await context.Activities
            .Include(a => a.ItineraryDay)
                .ThenInclude(d => d!.Trip)
                    .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(a => a.Id == activityId);

        if (activity?.ItineraryDay?.Trip == null) return null;

        var trip = activity.ItineraryDay.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return null;

        activity.Name = dto.Name;
        activity.Category = dto.Category;
        activity.Latitude = dto.Latitude;
        activity.Longitude = dto.Longitude;
        activity.Address = dto.Address;
        activity.StartTime = dto.StartTime;
        activity.EndTime = dto.EndTime;
        activity.BookingReference = dto.BookingReference;
        activity.Notes = dto.Notes;

        await context.SaveChangesAsync();

        return MapToResponseDto(activity);
    }

    public async Task<bool> DeleteActivityAsync(Guid activityId, Guid userId)
    {
        
        var activity = await context.Activities
            .Include(a => a.ItineraryDay)
                .ThenInclude(d => d!.Trip)
                    .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(a => a.Id == activityId);

        if (activity?.ItineraryDay?.Trip == null) return false;

        var trip = activity.ItineraryDay.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return false;

        activity.IsDeleted = true;
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ReorderActivitiesAsync(Guid dayId, Guid userId, ReorderActivitiesDto dto)
    {
       
        var day = await context.ItineraryDays
            .Include(d => d.Trip)
                .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(d => d.Id == dayId);

        if (day?.Trip == null) return false;

        var trip = day.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return false;

        var activities = await context.Activities
            .Where(a => a.ItineraryDayId == dayId)
            .ToListAsync();

        var itemsMap = dto.Items.ToDictionary(i => i.ActivityId, i => i.NewOrderIndex);

        foreach (var activity in activities)
        {
            if (itemsMap.TryGetValue(activity.Id, out var newIndex))
            {
                activity.OrderIndex = newIndex;
            }
        }

        await context.SaveChangesAsync();
        return true;
    }

    private static ActivityResponseDto MapToResponseDto(Activity a) => new()
    {
        Id = a.Id,
        ItineraryDayId = a.ItineraryDayId,
        Name = a.Name,
        Category = a.Category,
        Latitude = a.Latitude,
        Longitude = a.Longitude,
        Address = a.Address,
        StartTime = a.StartTime,
        EndTime = a.EndTime,
        OrderIndex = a.OrderIndex,
        BookingReference = a.BookingReference,
        Notes = a.Notes
    };
}
