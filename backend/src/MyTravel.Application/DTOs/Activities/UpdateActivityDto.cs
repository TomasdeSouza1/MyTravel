using System;
using MyTravel.Domain.Enums;

namespace MyTravel.Application.DTOs.Activities;

public class UpdateActivityDto
{
    public string Name { get; set; } = "";                                                                       
    public ActivityCategory Category { get; set; }                                                               
    public double Latitude { get; set; }                                                                         
    public double Longitude { get; set; }                                                                        
    public string? Address { get; set; }                                                                         
    public TimeOnly StartTime { get; set; }                                                                      
    public TimeOnly EndTime { get; set; }                                                                        
    public string? BookingReference { get; set; }                                                                
    public string? Notes { get; set; }      
}
