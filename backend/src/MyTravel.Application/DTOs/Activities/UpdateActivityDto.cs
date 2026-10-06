using System;
using MyTravel.Domain.Enums;

namespace MyTravel.Application.DTOs.Activities;

public sealed record UpdateActivityDto
{
    public string Name { get; init; } = "";                                                                       
    public ActivityCategory Category { get; init; }                                                               
    public double Latitude { get; init; }                                                                         
    public double Longitude { get; init; }                                                                        
    public string? Address { get; init; }                                                                         
    public TimeOnly StartTime { get; init; }                                                                      
    public TimeOnly EndTime { get; init; }                                                                        
    public string? BookingReference { get; init; }                                                                
    public string? Notes { get; init; }      
}
