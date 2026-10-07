using MyTravel.Application.DTOs.Auth;
using MyTravel.Application.Validators.AuthValidators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests;

public class LoginRequestDtoValidatorTests
{
    private readonly LoginRequestDtoValidator _validator = new();

    private static LoginRequestDto CreateValidDto() => new()
    {
        Email = "test@example.com",
        Password = "Password123"
    };

    [Fact]
    public void Validate_ValidLogin_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyEmail_ShouldHaveValidationError(string email)
    {
        var dto = CreateValidDto() with { Email = email };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginRequestDto.Email));
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("test@")]
    [InlineData("@example.com")]
    public void Validate_InvalidEmailFormat_ShouldHaveValidationError(string email)
    {
        var dto = CreateValidDto() with { Email = email };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginRequestDto.Email));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyPassword_ShouldHaveValidationError(string password)
    {
        var dto = CreateValidDto() with { Password = password };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginRequestDto.Password));
    }
}
