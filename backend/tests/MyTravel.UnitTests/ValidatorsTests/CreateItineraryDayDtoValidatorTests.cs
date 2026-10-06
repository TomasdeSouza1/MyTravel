using MyTravel.Application.DTOs.ItineraryDays;
using MyTravel.Application.Validators.ItineraryValidators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests;

public class CreateItineraryDayDtoValidatorTests
{
    private readonly CreateItineraryDayDtoValidator _validator = new();

    private CreateItineraryDayDto CreateValidDto() => new()
    {
        DayNumber = 1,
        Date = new DateOnly(2026, 11, 1),
        LocationCountry = "España",
        LocationCity = "Madrid",
        WeatherSumm = "Soleado",
        TemperatureC = 22.5m,
        Notes = "Llegada al hotel por la mañana"
    };

    [Fact]
    public void Validate_ValidDay_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidDayNumber_ShouldHaveValidationError(int dayNumber)
    {
        var dto = CreateValidDto();
        dto.DayNumber = dayNumber;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateItineraryDayDto.DayNumber));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyLocationCountry_ShouldHaveValidationError(string country)
    {
        var dto = CreateValidDto();
        dto.LocationCountry = country;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateItineraryDayDto.LocationCountry));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyLocationCity_ShouldHaveValidationError(string city)
    {
        var dto = CreateValidDto();
        dto.LocationCity = city;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateItineraryDayDto.LocationCity));
    }

    [Theory]
    [InlineData(-61.0)]
    [InlineData(61.0)]
    public void Validate_TemperatureOutOfRange_ShouldHaveValidationError(decimal temp)
    {
        var dto = CreateValidDto();
        dto.TemperatureC = temp;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateItineraryDayDto.TemperatureC));
    }

    [Fact]
    public void Validate_NullTemperature_ShouldPass()
    {
        var dto = CreateValidDto();
        dto.TemperatureC = null;

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
