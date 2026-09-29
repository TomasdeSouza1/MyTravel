using System;
using MyTravel.Application.DTOs.Activities;

namespace MyTravel.Application.DTOs.ItineraryDays;

public class ItineraryDayResponseDto
{
    public Guid Id { get; set; }                                                                                 
    public Guid TripId { get; set; }                                                                             
    public int DayNumber { get; set; }                                                                           
    public DateOnly Date { get; set; }                                                                           
    public string LocationCountry { get; set; } = "";                                                            
    public string LocationCity { get; set; } = "";                                                               
    public string? WeatherSumm { get; set; }                                                                     
    public decimal? TemperatureC { get; set; }                                                                   
    public string? Notes { get; set; }                                                                           
                                                                                                                     
    public List<ActivityResponseDto> Activities { get; set; } = [];  
}
