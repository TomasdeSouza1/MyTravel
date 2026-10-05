using MyTravel.Application.DTOs.Trips;
using MyTravel.Application.Validators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests;

public class CreateTripDtoValidatorTests
{
    private readonly CreateTripDtoValidator _validator = new();

    private CreateTripDto CreateValidDto() => new()
    {
        Title = "Viaje a Japón",
        Description = "Recorrido por Tokio y Kioto",
        DestinationCountry = "Japón",
        DestinationCity = "Tokio",
        BaseCurrency = "USD",
        StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
        EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20)),
        TotalBudget = 3500.00m,
        CoverImageUrl = "https://example.com/japan.jpg"
    };

    [Fact]
    public void Validate_ValidTrip_ShouldNotHaveErrors()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")] // Menor a 3 caracteres
    public void Validate_InvalidTitle_ShouldHaveValidationError(string title)
    {
        var dto = CreateValidDto();
        dto.Title = title;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateTripDto.Title));
    }

    [Theory]
    [InlineData("us")]    // minúsculas
    [InlineData("US")]    // 2 letras
    [InlineData("USDD")]  // 4 letras
    [InlineData("123")]   // números
    public void Validate_InvalidBaseCurrency_ShouldHaveValidationError(string currency)
    {
        var dto = CreateValidDto();
        dto.BaseCurrency = currency;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateTripDto.BaseCurrency));
    }

    [Fact]
    public void Validate_NegativeBudget_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.TotalBudget = -10;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateTripDto.TotalBudget));
    }

    [Fact]
    public void Validate_EndDateBeforeStartDate_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.StartDate = new DateOnly(2026, 10, 15);
        dto.EndDate = new DateOnly(2026, 10, 10);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateTripDto.EndDate));
    }

    [Fact]
    public void Validate_TripExceeds50Days_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.StartDate = new DateOnly(2026, 1, 1);
        dto.EndDate = new DateOnly(2026, 3, 1); // 59 días

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateTripDto.EndDate));
    }

    [Fact]
    public void Validate_InvalidCoverImageUrl_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.CoverImageUrl = "not-a-valid-url";

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateTripDto.CoverImageUrl));
    }
}
