using System;
using MyTravel.Domain.Enums;


namespace MyTravel.Application.DTOs.Activities;

public class ActivityResponseDto
{
    public Guid Id { get; set; }                                                                                 
    public Guid ItineraryDayId { get; set; }                                                                     
    public string Name { get; set; } = "";                                                                       
    public ActivityCategory Category { get; set; }                                                               
    public double Latitude { get; set; }                                                                         
    public double Longitude { get; set; }                                                                        
    public string? Address { get; set; }                                                                         
    public TimeOnly StartTime { get; set; }                                                                      
    public TimeOnly EndTime { get; set; }                                                                        
    public int OrderIndex { get; set; }                                                                          
    public string? BookingReference { get; set; }                                                                
    public string? Notes { get; set; }
}
