using System;

namespace MyTravel.Application.DTOs.Trips;

public class TripResponseDto
{
    public Guid Id { get; set; }                                                                                 
    public Guid UserId { get; set; }                                                                             
     public string Title { get; set; } = "";                                                                      
    public string? Description { get; set; }                                                                     
    public string DestinationCountry { get; set; } = "";                                                         
    public string DestinationCity { get; set; } = "";                                                            
    public string? CoverImageUrl { get; set; }                                                                   
    public string BaseCurrency { get; set; } = "USD";                                                            
    public DateOnly StartDate { get; set; }                                                                      
    public DateOnly EndDate { get; set; }                                                                        
    public decimal TotalBudget { get; set; }                                                                     
    public string InviteToken { get; set; } = "";                                                                
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }   
}
