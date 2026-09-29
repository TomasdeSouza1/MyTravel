using System;

namespace MyTravel.Application.DTOs.ItineraryDays;

public class CreateItineraryDayDto
{
    public int DayNumber { get; set; }                                                                           
    public DateOnly Date { get; set; }                                                                           
    public string LocationCountry { get; set; } = "";                                                            
    public string LocationCity { get; set; } = "";                                                               
    public string? WeatherSumm { get; set; }                                                                     
    public decimal? TemperatureC { get; set; }                                                                   
    public string? Notes { get; set; } 
}
