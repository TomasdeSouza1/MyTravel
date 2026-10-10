using System;

namespace MyTravel.Application.DTOs.Flights;

public sealed record UpdateFlightDto
{
    public string Airline { get; init; } = "";
    public string FlightNumber { get; init; } = "";
    public string DepartureAirport { get; init; } = "";
    public string ArrivalAirport { get; init; } = "";
    public DateTime DepartureTime { get; init; }
    public DateTime ArrivalTime { get; init; }
    public string? BookingReference { get; init; }
    public string? Terminal { get; init; }
    public string? Gate { get; init; }
    public string? SeatNumber { get; init; }
    public string? Notes { get; init; }
}
