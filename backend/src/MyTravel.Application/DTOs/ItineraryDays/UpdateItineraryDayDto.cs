using System;

namespace MyTravel.Application.DTOs.ItineraryDays;

public class UpdateItineraryDayDto
{
    public string LocationCountry { get; set; } = "";                                                            
    public string LocationCity { get; set; } = "";                                                               
    public string? WeatherSumm { get; set; }                                                                     
    public decimal? TemperatureC { get; set; }                                                                   
    public string? Notes { get; set; } 
}
