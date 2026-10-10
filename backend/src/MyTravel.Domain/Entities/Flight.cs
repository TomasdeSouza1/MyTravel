namespace MyTravel.Domain.Entities;

public class Flight : BaseEntity
{
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    public string Airline { get; set; } = "";
    public string FlightNumber { get; set; } = "";
    public string DepartureAirport { get; set; } = ""; 
    public string ArrivalAirport { get; set; } = ""; 
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string? BookingReference { get; set; }
    public string? Terminal { get; set; }
    public string? Gate { get; set; }
    public string? SeatNumber { get; set; }
    public string? Notes { get; set; }
}
