using Microsoft.EntityFrameworkCore;
using MyTravel.Application.DTOs.Activities;
using MyTravel.Application.DTOs.ItineraryDays;
using MyTravel.Application.Interfaces.Itinerary;
using MyTravel.Domain.Entities;
using MyTravel.Domain.Enums;
using MyTravel.Infrastructure.Persistence;

namespace MyTravel.Infrastructure.Services.Itineraries;

public class ItineraryDayService(ApplicationDbContext context) : IItineraryDayService
{
    public async Task<ItineraryDayResponseDto?> AddDayAsync(Guid tripId, Guid userId, CreateItineraryDayDto dto)
    {
        var trip = await context.Trips
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return null;

        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return null;

        var day = new ItineraryDay
        {
            TripId = tripId,
            DayNumber = dto.DayNumber,
            Date = dto.Date,
            LocationCountry = dto.LocationCountry,
            LocationCity = dto.LocationCity,
            WeatherSumm = dto.WeatherSumm,
            TemperatureC = dto.TemperatureC,
            Notes = dto.Notes
        };

        context.ItineraryDays.Add(day);
        await context.SaveChangesAsync();

        return MapToResponseDto(day);
    }

    public async Task<ItineraryDayResponseDto?> UpdateDayAsync(Guid dayId, Guid userId, UpdateItineraryDayDto dto)
    {
        
        var day = await context.ItineraryDays
            .Include(d => d.Activities.OrderBy(a => a.OrderIndex))
            .Include(d => d.Trip)
                .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(d => d.Id == dayId);

        if (day?.Trip == null) return null;

        var trip = day.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return null;

        day.LocationCountry = dto.LocationCountry;
        day.LocationCity = dto.LocationCity;
        day.WeatherSumm = dto.WeatherSumm;
        day.TemperatureC = dto.TemperatureC;
        day.Notes = dto.Notes;

        await context.SaveChangesAsync();

        return MapToResponseDto(day);
    }

    public async Task<bool> DeleteDayAsync(Guid dayId, Guid userId)
    {
        
        var day = await context.ItineraryDays
            .Include(d => d.Activities)
            .Include(d => d.Trip)
                .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(d => d.Id == dayId);

        if (day?.Trip == null) return false;

        var trip = day.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return false;

        day.IsDeleted = true;
        foreach (var activity in day.Activities)
        {
            activity.IsDeleted = true;
        }

        await context.SaveChangesAsync();
        return true;
    }

    private static ItineraryDayResponseDto MapToResponseDto(ItineraryDay day) => new()
    {
        Id = day.Id,
        TripId = day.TripId,
        DayNumber = day.DayNumber,
        Date = day.Date,
        LocationCountry = day.LocationCountry,
        LocationCity = day.LocationCity,
        WeatherSumm = day.WeatherSumm,
        TemperatureC = day.TemperatureC,
        Notes = day.Notes,
        Activities = [.. day.Activities.Select(a => new ActivityResponseDto
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
        })]
    };
}
