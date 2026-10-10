using MyTravel.Application.DTOs.Activities;
using MyTravel.Application.Validators.ActivityValidators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests.ActivityValidators;

public class ReorderActivitiesDtoValidatorTests
{
    private readonly ReorderActivitiesDtoValidator _validator = new();

    [Fact]
    public void Validate_ValidReorderDto_ShouldPass()
    {
        var dto = new ReorderActivitiesDto
        {
            Items =
            [
                new ActivityOrderItemDto { ActivityId = Guid.NewGuid(), NewOrderIndex = 0 },
                new ActivityOrderItemDto { ActivityId = Guid.NewGuid(), NewOrderIndex = 1 }
            ]
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_EmptyItemsList_ShouldHaveValidationError()
    {
        var dto = new ReorderActivitiesDto { Items = [] };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ReorderActivitiesDto.Items));
    }

    [Fact]
    public void Validate_EmptyActivityIdInItem_ShouldHaveValidationError()
    {
        var dto = new ReorderActivitiesDto
        {
            Items =
            [
                new ActivityOrderItemDto { ActivityId = Guid.Empty, NewOrderIndex = 0 }
            ]
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("ID de la actividad"));
    }

    [Fact]
    public void Validate_NegativeOrderIndexInItem_ShouldHaveValidationError()
    {
        var dto = new ReorderActivitiesDto
        {
            Items =
            [
                new ActivityOrderItemDto { ActivityId = Guid.NewGuid(), NewOrderIndex = -1 }
            ]
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("índice debe ser mayor o igual a 0"));
    }
}
