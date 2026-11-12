using System;
using MyTravel.Application.DTOs.Collaboration;
using MyTravel.Application.Validators.CollaborationValidators;
using MyTravel.Domain.Entities;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests.CollaborationValidators;

public class JoinTripDtoValidatorsTest
{
    private readonly JoinTripValidator _validator = new();

    private static JointripDto CreateValidDto() => new()
    {
        InviteToken = Trip.GenerateInviteToken()
    };

    [Fact]
    public void Validate_ValidGeneratedInviteToken_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyInviteToken_ShouldHaveValidationError(string token)
    {
        var dto = CreateValidDto() with { InviteToken = token };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(JointripDto.InviteToken));
    }

    [Fact]
    public void Validate_InviteTokenExceedsMaxLength_ShouldHaveValidationError()
    {
        var dto = CreateValidDto() with { InviteToken = new string('a', 65) };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(JointripDto.InviteToken));
    }

    [Fact]
    public void Validate_InviteTokenWithinMaxLength_ShouldPass()
    {
        var dto = CreateValidDto() with { InviteToken = new string('a', 64) };
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("token-con-guiones")]
    [InlineData("token con espacios")]
    [InlineData("token@#$!")]
    [InlineData("token_con_guion_bajo")]
    public void Validate_NonAlphanumericInviteToken_ShouldHaveValidationError(string token)
    {
        var dto = CreateValidDto() with { InviteToken = token };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(JointripDto.InviteToken));
    }
}
