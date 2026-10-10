using System;
using MyTravel.Application.DTOs.Flights;
using MyTravel.Application.Validators.FlightValidators;
using Xunit;

namespace MyTravel.UnitTests.ValidatorsTests.FlightValidators;

public class UpdateFlightDtoValidatorTests
{
    private readonly UpdateFlightDtoValidator _validator = new();

    private UpdateFlightDto CreateValidDto() => new()
    {
        Airline = "Air France",
        FlightNumber = "AF417",
        DepartureAirport = "CDG",
        ArrivalAirport = "JFK",
        DepartureTime = DateTime.UtcNow.AddDays(15),
        ArrivalTime = DateTime.UtcNow.AddDays(15).AddHours(8),
        BookingReference = "RESERVATION123",
        Terminal = "2E",
        Gate = "K45",
        SeatNumber = "22C",
        Notes = "Equipaje extra contratado."
    };

    [Fact]
    public void Validate_ValidUpdate_ShouldPass()
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
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateFlightDto.Airline));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyFlightNumber_ShouldHaveValidationError(string flightNumber)
    {
        var dto = CreateValidDto() with { FlightNumber = flightNumber };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateFlightDto.FlightNumber));
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("TOOLONG")]
    [InlineData("123")]
    public void Validate_InvalidAirportCodes_ShouldHaveValidationError(string invalidAirport)
    {
        var departureDto = CreateValidDto() with { DepartureAirport = invalidAirport };
        var depResult = _validator.Validate(departureDto);

        Assert.False(depResult.IsValid);
        Assert.Contains(depResult.Errors, e => e.PropertyName == nameof(UpdateFlightDto.DepartureAirport));

        var arrivalDto = CreateValidDto() with { ArrivalAirport = invalidAirport };
        var arrResult = _validator.Validate(arrivalDto);

        Assert.False(arrResult.IsValid);
        Assert.Contains(arrResult.Errors, e => e.PropertyName == nameof(UpdateFlightDto.ArrivalAirport));
    }

    [Theory]
    [InlineData("CDG", "cdg")]
    [InlineData("JFK", "JFK")]
    public void Validate_ArrivalAirportEqualsDepartureAirport_ShouldHaveValidationError(string departure, string arrival)
    {
        var dto = CreateValidDto() with
        {
            DepartureAirport = departure,
            ArrivalAirport = arrival
        };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateFlightDto.ArrivalAirport) &&
            e.ErrorMessage == "El aeropuerto de llegada no puede ser igual al de salida.");
    }

    [Fact]
    public void Validate_ArrivalTimeEqualOrBeforeDepartureTime_ShouldHaveValidationError()
    {
        var departure = DateTime.UtcNow.AddDays(20);

        var dtoBefore = CreateValidDto() with
        {
            DepartureTime = departure,
            ArrivalTime = departure.AddHours(-2)
        };
        var resultBefore = _validator.Validate(dtoBefore);
        Assert.False(resultBefore.IsValid);
        Assert.Contains(resultBefore.Errors, e => e.PropertyName == nameof(UpdateFlightDto.ArrivalTime));

        var dtoEqual = CreateValidDto() with
        {
            DepartureTime = departure,
            ArrivalTime = departure
        };
        var resultEqual = _validator.Validate(dtoEqual);
        Assert.False(resultEqual.IsValid);
        Assert.Contains(resultEqual.Errors, e => e.PropertyName == nameof(UpdateFlightDto.ArrivalTime));
    }

    [Fact]
    public void Validate_NotesExceeding100Chars_ShouldHaveValidationError()
    {
        var dto = CreateValidDto() with { Notes = new string('X', 101) };
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateFlightDto.Notes));
    }

    [Fact]
    public void Validate_NotesWithin100Chars_ShouldPass()
    {
        var dto = CreateValidDto() with { Notes = new string('X', 100) };
        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
