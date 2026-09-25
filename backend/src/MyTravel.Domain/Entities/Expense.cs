using MyTravel.Domain.Enums;

namespace MyTravel.Domain.Entities;

public class Expense : BaseEntity
{
    public Guid TripId { get; set; }
    public Guid? ActivityId { get; set; }
    public decimal OriginalAmount { get; set; }
    public string OriginalCurrency { get; set; } = "USD";
    public decimal ExchangeRateUsed { get; set; } = 1.0m;
    public decimal ConvertedAmount { get; set; }
    public ExpenseCategory Category { get; set; }
    public DateOnly Date { get; set; }

}
