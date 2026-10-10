using MyTravel.Application.DTOs.Auth;
using MyTravel.Application.Validators.AuthValidators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests.AuthValidators;

public class RegisterRequestDtoValidatorTests
{
    private readonly RegisterRequestDtoValidator _validator = new();

    private static RegisterRequestDto CreateValidDto() => new()
    {
        FullName = "Tomas De Souza",
        Email = "tomas@example.com",
        Password = "SecurePassword123"
    };

    [Fact]
    public void Validate_ValidRegister_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyFullName_ShouldHaveValidationError(string fullName)
    {
        var dto = CreateValidDto() with { FullName = fullName };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterRequestDto.FullName));
    }

    [Fact]
    public void Validate_FullNameExceeds100Chars_ShouldHaveValidationError()
    {
        var dto = CreateValidDto() with { FullName = new string('A', 101) };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterRequestDto.FullName));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyEmail_ShouldHaveValidationError(string email)
    {
        var dto = CreateValidDto() with { Email = email };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterRequestDto.Email));
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
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterRequestDto.Email));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyPassword_ShouldHaveValidationError(string password)
    {
        var dto = CreateValidDto() with { Password = password };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterRequestDto.Password));
    }

    [Theory]
    [InlineData("123")]
    [InlineData("123456789")] // 9 caracteres (el mínimo es 10)
    public void Validate_PasswordLessThan10Chars_ShouldHaveValidationError(string password)
    {
        var dto = CreateValidDto() with { Password = password };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterRequestDto.Password));
    }
}
