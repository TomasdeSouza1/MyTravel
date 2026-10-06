using System;

namespace MyTravel.Application.DTOs.Trips;

public sealed record CreateTripDto
{
    public string Title { get; init; } = ""; 
    public string? Description { get; init; }
    public string DestinationCountry { get; init; } = ""; 
    public string DestinationCity { get; init; } = "";
    public string? CoverImageUrl { get; init; }   
    public string BaseCurrency { get; init; } = "USD";                                                            
    public DateOnly StartDate { get; init; }                                                                      
    public DateOnly EndDate { get; init; }                                                                        
    public decimal TotalBudget { get; init; }
    public bool IsPublic { get; init; } = false;                                                  
}
