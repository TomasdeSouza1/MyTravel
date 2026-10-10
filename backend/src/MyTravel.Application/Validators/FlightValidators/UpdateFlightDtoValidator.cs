using System;
using FluentValidation;
using MyTravel.Application.DTOs.Flights;

namespace MyTravel.Application.Validators.FlightValidators;

public class UpdateFlightDtoValidator : AbstractValidator<UpdateFlightDto>
{
    public UpdateFlightDtoValidator()
    {
        RuleFor(x => x.Airline)
            .NotEmpty().WithMessage("La aerolínea es obligatoria.")
            .MaximumLength(100).WithMessage("La aerolínea no puede superar los 100 caracteres.");

        RuleFor(x => x.FlightNumber)
            .NotEmpty().WithMessage("El número de vuelo es obligatorio.")
            .MaximumLength(20).WithMessage("El número de vuelo no puede superar los 20 caracteres.");

        RuleFor(x => x.DepartureAirport)
            .NotEmpty().WithMessage("El aeropuerto de salida es obligatorio.")
            .Length(3, 4).WithMessage("El código de aeropuerto de salida debe tener entre 3 y 4 caracteres (ej. EZE).")
            .Matches("^[A-Za-z]{3,4}$").WithMessage("El código de aeropuerto debe contener solo letras.");

        RuleFor(x => x.ArrivalAirport)
            .NotEmpty().WithMessage("El aeropuerto de llegada es obligatorio.")
            .Length(3, 4).WithMessage("El código de aeropuerto de llegada debe tener entre 3 y 4 caracteres (ej. MAD).")
            .Matches("^[A-Za-z]{3,4}$").WithMessage("El código de aeropuerto debe contener solo letras.")
            .Must((dto, arrival) => !string.Equals(dto.DepartureAirport, arrival, StringComparison.OrdinalIgnoreCase))
            .WithMessage("El aeropuerto de llegada no puede ser igual al de salida.");

        RuleFor(x => x.DepartureTime)
            .NotEmpty().WithMessage("La fecha y hora de salida es obligatoria.");

        RuleFor(x => x.ArrivalTime)
            .NotEmpty().WithMessage("La fecha y hora de llegada es obligatoria.")
            .GreaterThan(x => x.DepartureTime).WithMessage("La fecha y hora de llegada debe ser posterior a la de salida.");

        RuleFor(x => x.BookingReference)
            .MaximumLength(50).WithMessage("El código de reserva no puede superar los 50 caracteres.");

        RuleFor(x => x.Terminal)
            .MaximumLength(20).WithMessage("La terminal no puede superar los 20 caracteres.");

        RuleFor(x => x.Gate)
            .MaximumLength(20).WithMessage("La puerta de embarque no puede superar los 20 caracteres.");

        RuleFor(x => x.SeatNumber)
            .MaximumLength(10).WithMessage("El número de asiento no puede superar los 10 caracteres.");

        RuleFor(x => x.Notes)
            .MaximumLength(100).WithMessage("Las notas no pueden superar los 100 caracteres.");
    }
}
