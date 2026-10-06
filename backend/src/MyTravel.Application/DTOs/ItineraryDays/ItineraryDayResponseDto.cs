using System;
using MyTravel.Application.DTOs.Activities;

namespace MyTravel.Application.DTOs.ItineraryDays;

public sealed record ItineraryDayResponseDto
{
    public Guid Id { get; init; }                                                                                 
    public Guid TripId { get; init; }                                                                             
    public int DayNumber { get; init; }                                                                           
    public DateOnly Date { get; init; }                                                                           
    public string LocationCountry { get; init; } = "";                                                            
    public string LocationCity { get; init; } = "";                                                               
    public string? WeatherSumm { get; init; }                                                                     
    public decimal? TemperatureC { get; init; }                                                                   
    public string? Notes { get; init; }                                                                           
                                                                                                                     
    public List<ActivityResponseDto> Activities { get; init; } = [];  
}
