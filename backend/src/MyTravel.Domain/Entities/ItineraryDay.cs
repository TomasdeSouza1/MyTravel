namespace MyTravel.Domain.Entities;

public class ItineraryDay : BaseEntity
{
    public Guid TripId { get; set; } 
    public int DayNumber { get; set; }
    public DateOnly Date { get; set; }
    public string LocationCountry { get; set; } = "";
    public string LocationCity { get; set; } = "";
    public string? WeatherSumm { get; set; }
    public decimal? TemperatureC { get; set; }
    public string? Notes { get; set; }

    public List<Activity> Activities { get; set; } = [];
}
