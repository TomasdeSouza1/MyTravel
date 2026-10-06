using System;

namespace MyTravel.Application.DTOs.ItineraryDays;

public sealed record CreateItineraryDayDto
{
    public int DayNumber { get; init; }                                                                           
    public DateOnly Date { get; init; }                                                                           
    public string LocationCountry { get; init; } = "";                                                            
    public string LocationCity { get; init; } = "";                                                               
    public string? WeatherSumm { get; init; }                                                                     
    public decimal? TemperatureC { get; init; }                                                                   
    public string? Notes { get; init; } 
}
