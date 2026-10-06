using MyTravel.Application.DTOs.Trips;
using MyTravel.Application.Validators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests;

public class UpdateTripDtoValidatorTests
{
    private readonly UpdateTripDtoValidator _validator = new();

    private UpdateTripDto CreateValidDto() => new()
    {
        Title = "Viaje a Roma",
        Description = "Recorrido por el Coliseo y el Vaticano",
        DestinationCountry = "Italia",
        DestinationCity = "Roma",
        BaseCurrency = "EUR",
        StartDate = new DateOnly(2026, 12, 1),
        EndDate = new DateOnly(2026, 12, 10),
        TotalBudget = 2500m,
        CoverImageUrl = "https://example.com/rome.jpg",
        IsPublic = true
    };

    [Fact]
    public void Validate_ValidTrip_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public void Validate_InvalidTitle_ShouldHaveValidationError(string title)
    {
        var dto = CreateValidDto();
        dto.Title = title;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateTripDto.Title));
    }

    [Fact]
    public void Validate_TitleExceeds120Chars_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Title = new string('A', 121);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateTripDto.Title));
    }

    [Theory]
    [InlineData("eur")]
    [InlineData("EU")]
    [InlineData("EURO")]
    public void Validate_InvalidCurrency_ShouldHaveValidationError(string currency)
    {
        var dto = CreateValidDto();
        dto.BaseCurrency = currency;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateTripDto.BaseCurrency));
    }

    [Fact]
    public void Validate_NegativeBudget_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.TotalBudget = -1;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateTripDto.TotalBudget));
    }

    [Fact]
    public void Validate_EndDateBeforeStartDate_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.StartDate = new DateOnly(2026, 12, 10);
        dto.EndDate = new DateOnly(2026, 12, 1);

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateTripDto.EndDate));
    }
}
