using MyTravel.Application.DTOs.Activities;
using MyTravel.Application.Validators.ActivityValidators;
using MyTravel.Domain.Enums;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests;

public class CreateActivityDtoValidatorTests
{
    private readonly CreateActivityDtoValidator _validator = new();

    private CreateActivityDto CreateValidDto() => new()
    {
        Name = "Visita al Museo del Prado",
        Category = ActivityCategory.Attraction,
        Latitude = 40.4138,
        Longitude = -3.6921,
        Address = "Calle Ruiz de Alarcón 23, Madrid",
        StartTime = new TimeOnly(10, 0),
        EndTime = new TimeOnly(12, 30),
        OrderIndex = 0,
        BookingReference = "BK-12345",
        Notes = "Comprar entradas anticipadas"
    };

    [Fact]
    public void Validate_ValidActivity_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")] // Menos de 2 caracteres
    public void Validate_InvalidName_ShouldHaveValidationError(string name)
    {
        var dto = CreateValidDto();
        dto.Name = name;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateActivityDto.Name));
    }

    [Theory]
    [InlineData(-91.0)]
    [InlineData(91.0)]
    public void Validate_InvalidLatitude_ShouldHaveValidationError(double lat)
    {
        var dto = CreateValidDto();
        dto.Latitude = lat;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateActivityDto.Latitude));
    }

    [Theory]
    [InlineData(-181.0)]
    [InlineData(181.0)]
    public void Validate_InvalidLongitude_ShouldHaveValidationError(double lng)
    {
        var dto = CreateValidDto();
        dto.Longitude = lng;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateActivityDto.Longitude));
    }

    [Fact]
    public void Validate_EndTimeBeforeStartTime_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.StartTime = new TimeOnly(14, 0);
        dto.EndTime = new TimeOnly(11, 0); // Anterior a StartTime

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateActivityDto.EndTime));
    }

    [Fact]
    public void Validate_NegativeOrderIndex_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.OrderIndex = -1;

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateActivityDto.OrderIndex));
    }
}
