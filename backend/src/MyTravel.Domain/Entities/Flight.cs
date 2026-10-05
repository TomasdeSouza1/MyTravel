namespace MyTravel.Domain.Entities;

public class Flight : BaseEntity
{
    public Guid TripId { get; set; }
    public string Airline { get; set; } = "";
    public string FlightNumber { get; set; } = "";
    public string DepartureAirport { get; set; } = ""; // Código IATA (ej. EZE, MAD, JFK)
    public string ArrivalAirport { get; set; } = "";   // Código IATA
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string? BookingReference { get; set; }
    public string? Terminal { get; set; }
    public string? Gate { get; set; }
    public string? Notes { get; set; }
}
