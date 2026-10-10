using MyTravel.Application.DTOs.Activities;
using MyTravel.Application.Validators.ActivityValidators;
using MyTravel.Domain.Enums;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests.ActivityValidators;

public class UpdateActivityDtoValidatorTests
{
    private readonly UpdateActivityDtoValidator _validator = new();

    private UpdateActivityDto CreateValidDto() => new()
    {
        Name = "Tour por la Torre Eiffel",
        Category = ActivityCategory.Attraction,
        Latitude = 48.8584,
        Longitude = 2.2945,
        Address = "Champ de Mars, París",
        StartTime = new TimeOnly(15, 0),
        EndTime = new TimeOnly(17, 0),
        BookingReference = "TOUR-999",
        Notes = "Llegar 15 min antes"
    };

    [Fact]
    public void Validate_ValidUpdateActivity_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public void Validate_InvalidName_ShouldHaveValidationError(string name)
    {
        var dto = CreateValidDto() with { Name = name };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateActivityDto.Name));
    }

    [Theory]
    [InlineData(-95.0)]
    [InlineData(95.0)]
    public void Validate_InvalidLatitude_ShouldHaveValidationError(double lat)
    {
        var dto = CreateValidDto() with { Latitude = lat };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateActivityDto.Latitude));
    }

    [Theory]
    [InlineData(-185.0)]
    [InlineData(185.0)]
    public void Validate_InvalidLongitude_ShouldHaveValidationError(double lng)
    {
        var dto = CreateValidDto() with { Longitude = lng };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateActivityDto.Longitude));
    }

    [Fact]
    public void Validate_EndTimeBeforeStartTime_ShouldHaveValidationError()
    {
        var dto = CreateValidDto() with
        {
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(16, 0)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateActivityDto.EndTime));
    }
}
