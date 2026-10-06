using MyTravel.Application.DTOs.ItineraryDays;
using MyTravel.Application.Validators.ItineraryValidators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests;

public class UpdateItineraryDayDtoValidatorTests
{
    private readonly UpdateItineraryDayDtoValidator _validator = new();

    private UpdateItineraryDayDto CreateValidDto() => new()
    {
        LocationCountry = "Francia",
        LocationCity = "París",
        WeatherSumm = "Parcialmente nublado",
        TemperatureC = 18.0m,
        Notes = "Día en el centro histórico"
    };

    [Fact]
    public void Validate_ValidUpdateDto_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
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
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateItineraryDayDto.LocationCountry));
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
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateItineraryDayDto.LocationCity));
    }

    [Theory]
    [InlineData(-65.0)]
    [InlineData(65.0)]
    public void Validate_TemperatureOutOfRange_ShouldHaveValidationError(decimal temp)
    {
        var dto = CreateValidDto();
        dto.TemperatureC = temp;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateItineraryDayDto.TemperatureC));
    }
}
