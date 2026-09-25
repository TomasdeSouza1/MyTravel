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
    public string InviteToken { get; set; } = Guid.NewGuid().ToString("N");

    public List<ItineraryDay> Days { get; set; } = [];
    public List<Expense> Expenses { get; set; } = [];
    public List<TripMember> Members { get; set; } = [];
}
