using System;
using MyTravel.Application.DTOs.Flights;
using MyTravel.Application.Validators.FlightValidators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests.FlightValidators;

public class CreateFlightDtoValidatorTests
{
    private readonly CreateFlightDtoValidator _validator = new();

    private CreateFlightDto CreateValidDto() => new()
    {
        Airline = "Iberia",
        FlightNumber = "IB6844",
        DepartureAirport = "EZE",
        ArrivalAirport = "MAD",
        DepartureTime = DateTime.UtcNow.AddDays(10),
        ArrivalTime = DateTime.UtcNow.AddDays(10).AddHours(12),
        BookingReference = "ABC123XYZ",
        Terminal = "T1",
        Gate = "B12",
        SeatNumber = "14A",
        Notes = "Vuelo directo nocturno, incluye equipaje de mano y bodega."
    };

    [Fact]
    public void Validate_ValidFlight_ShouldPass()
    {
        var dto = CreateValidDto();
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyAirline_ShouldHaveValidationError(string airline)
    {
        var dto = CreateValidDto() with { Airline = airline };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.Airline));
    }

    [Fact]
    public void Validate_AirlineExceedsMaxLength_ShouldHaveValidationError()
    {
        var dto = CreateValidDto() with { Airline = new string('A', 101) };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.Airline));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyFlightNumber_ShouldHaveValidationError(string flightNumber)
    {
        var dto = CreateValidDto() with { FlightNumber = flightNumber };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.FlightNumber));
    }

    [Fact]
    public void Validate_FlightNumberExceedsMaxLength_ShouldHaveValidationError()
    {
        var dto = CreateValidDto() with { FlightNumber = new string('F', 21) };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.FlightNumber));
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]      // Menor a 3 caracteres
    [InlineData("ABCDE")]   // Mayor a 4 caracteres
    [InlineData("123")]     // Números no permitidos
    [InlineData("EZ1")]     // Mezcla de letras y números
    [InlineData("EZ#")]     // Caracteres especiales
    public void Validate_InvalidDepartureAirport_ShouldHaveValidationError(string airport)
    {
        var dto = CreateValidDto() with { DepartureAirport = airport };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.DepartureAirport));
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]      // Menor a 3 caracteres
    [InlineData("ABCDE")]   // Mayor a 4 caracteres
    [InlineData("123")]     // Números no permitidos
    [InlineData("MA1")]     // Mezcla de letras y números
    [InlineData("MA$")]     // Caracteres especiales
    public void Validate_InvalidArrivalAirport_ShouldHaveValidationError(string airport)
    {
        var dto = CreateValidDto() with { ArrivalAirport = airport };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.ArrivalAirport));
    }

    [Theory]
    [InlineData("EZE", "EZE")]
    [InlineData("eze", "EZE")]
    [InlineData("EZE", "eze")]
    [InlineData("mad", "MAD")]
    public void Validate_ArrivalAirportEqualsDepartureAirportCaseInsensitive_ShouldHaveValidationError(string departure, string arrival)
    {
        var dto = CreateValidDto() with
        {
            DepartureAirport = departure,
            ArrivalAirport = arrival
        };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.ArrivalAirport) &&
            e.ErrorMessage == "El aeropuerto de llegada no puede ser igual al de salida.");
    }

    [Fact]
    public void Validate_ArrivalTimeBeforeDepartureTime_ShouldHaveValidationError()
    {
        var departure = DateTime.UtcNow.AddDays(5);
        var dto = CreateValidDto() with
        {
            DepartureTime = departure,
            ArrivalTime = departure.AddHours(-1)
        };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.ArrivalTime));
    }

    [Fact]
    public void Validate_ArrivalTimeEqualsDepartureTime_ShouldHaveValidationError()
    {
        var departure = DateTime.UtcNow.AddDays(5);
        var dto = CreateValidDto() with
        {
            DepartureTime = departure,
            ArrivalTime = departure
        };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.ArrivalTime));
    }

    [Fact]
    public void Validate_NotesExceeding100Chars_ShouldHaveValidationError()
    {
        var dto = CreateValidDto() with { Notes = new string('N', 101) };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFlightDto.Notes));
    }

    [Fact]
    public void Validate_NotesWithin100Chars_ShouldPass()
    {
        var dto = CreateValidDto() with { Notes = new string('N', 100) };
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
