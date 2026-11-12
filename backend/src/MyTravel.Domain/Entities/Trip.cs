using System.Security.Cryptography;
namespace MyTravel.Domain.Entities;

public class Trip : BaseEntity
{
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
    public string InviteToken { get; set; } = GenerateInviteToken();
    public bool IsPublic { get; set; } = false;

    public List<ItineraryDay> Days { get; set; } = [];
    public List<Expense> Expenses { get; set; } = [];
    public List<TripMember> Members { get; set; } = [];
    public List<Flight> Flights { get; set; } = [];
    
    public static string GenerateInviteToken() => RandomNumberGenerator.GetHexString(32, lowercase: true);

    public string RegenerateInviteToken()
    {
        InviteToken = GenerateInviteToken();
        return InviteToken;
    }


}
