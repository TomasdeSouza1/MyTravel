using System;
using MyTravel.Application.DTOs.Flights;
using MyTravel.Application.Interfaces.Flights;
using MyTravel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyTravel.Domain.Entities;
using MyTravel.Domain.Enums;

namespace MyTravel.Infrastructure.Services.Flights;

public class FlightService(ApplicationDbContext context) : IFlightService
{
    public async Task<IEnumerable<FlightResponseDto>?> GetFlightsByTripIdAsync(Guid tripId, Guid? userId)
    {
        var trip = await context.Trips
            .AsNoTracking()
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return null;

        var isOwnerOrMember = userId.HasValue && (trip.UserId == userId.Value || trip.Members.Any(m => m.UserId == userId.Value));
        if (!trip.IsPublic && !isOwnerOrMember) return null;

        var flights = await context.Flights
            .AsNoTracking()
            .Where(f => f.TripId == tripId)
            .OrderBy(f => f.DepartureTime)
            .ToListAsync();
        
        return flights.Select(f => MapToResponseDto(f, isOwnerOrMember));
    }

    public async Task<FlightResponseDto?> GetFlightByIdAsync(Guid flightId, Guid? userId)
    {
        var flight = await context.Flights
            .AsNoTracking()
            .Include(f => f.Trip)
                .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(f => f.Id == flightId);

        if (flight?.Trip == null) return null;

        var trip = flight.Trip;
        var isOwnerOrMember = userId.HasValue && (trip.UserId == userId.Value || trip.Members.Any(m => m.UserId == userId.Value));
        if (!trip.IsPublic && !isOwnerOrMember) return null;

        return MapToResponseDto(flight, isOwnerOrMember);
    }

    public async Task<FlightResponseDto?> AddFlightAsync(Guid tripId, Guid userId, CreateFlightDto dto)
    {
        var trip = await context.Trips
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return null;

        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return null;             
        
        var flight = new Flight
        {
            TripId = tripId,
            Airline = dto.Airline,
            FlightNumber = dto.FlightNumber,
            DepartureAirport = dto.DepartureAirport.ToUpperInvariant(),
            ArrivalAirport = dto.ArrivalAirport.ToUpperInvariant(),
            DepartureTime = DateTime.SpecifyKind(dto.DepartureTime, DateTimeKind.Utc),
            ArrivalTime = DateTime.SpecifyKind(dto.ArrivalTime, DateTimeKind.Utc),
            BookingReference = dto.BookingReference,
            Terminal = dto.Terminal,
            Gate = dto.Gate,
            SeatNumber = dto.SeatNumber,
            Notes = dto.Notes
        };
        context.Flights.Add(flight);
        await context.SaveChangesAsync();
        return MapToResponseDto(flight);
    }

    public async Task<FlightResponseDto?> UpdateFlightAsync(Guid flightId, Guid userId, UpdateFlightDto dto)
    {
        var flight = await context.Flights
            .Include(f => f.Trip)
                .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(f => f.Id == flightId);

        if (flight?.Trip == null) return null;

        var trip = flight.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return null;

        flight.Airline = dto.Airline;
        flight.FlightNumber = dto.FlightNumber;
        flight.DepartureAirport = dto.DepartureAirport.ToUpperInvariant();
        flight.ArrivalAirport = dto.ArrivalAirport.ToUpperInvariant();
        flight.DepartureTime = DateTime.SpecifyKind(dto.DepartureTime, DateTimeKind.Utc);
        flight.ArrivalTime = DateTime.SpecifyKind(dto.ArrivalTime, DateTimeKind.Utc);
        flight.BookingReference = dto.BookingReference;
        flight.Terminal = dto.Terminal;
        flight.Gate = dto.Gate;
        flight.SeatNumber = dto.SeatNumber;
        flight.Notes = dto.Notes;

        await context.SaveChangesAsync();
        return MapToResponseDto(flight);
    }

    public async Task<bool> DeleteFlightAsync(Guid flightId, Guid userId)
    {
        var flight = await context.Flights
            .Include(f => f.Trip)
                .ThenInclude(t => t!.Members)
            .FirstOrDefaultAsync(f => f.Id == flightId);

        if (flight?.Trip == null) return false;

        var trip = flight.Trip;
        var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));
        if (!canEdit) return false;
         
        flight.IsDeleted = true;

        await context.SaveChangesAsync();
        return true;
    }

    private static FlightResponseDto MapToResponseDto(Flight flight, bool includePrivateData = true) => new()
    {
        Id = flight.Id,
        TripId = flight.TripId,
        Airline = flight.Airline,
        FlightNumber = flight.FlightNumber,
        DepartureAirport = flight.DepartureAirport,
        ArrivalAirport = flight.ArrivalAirport,
        DepartureTime = flight.DepartureTime,
        ArrivalTime = flight.ArrivalTime,
        BookingReference = includePrivateData ? flight.BookingReference : null,
        Terminal = flight.Terminal,
        Gate = flight.Gate,
        SeatNumber = flight.SeatNumber,
        Notes = flight.Notes
    };
}
