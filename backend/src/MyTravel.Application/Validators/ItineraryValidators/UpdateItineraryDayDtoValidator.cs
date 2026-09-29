using System;
using FluentValidation;
using MyTravel.Application.DTOs.ItineraryDays;

namespace MyTravel.Application.Validators.ItineraryValidators;

public class UpdateItineraryDayDtoValidator : AbstractValidator<UpdateItineraryDayDto>
{
    public UpdateItineraryDayDtoValidator()                                                                      
        {                                                                                                            
            RuleFor(x => x.LocationCountry)                                                                          
                .NotEmpty().WithMessage("El país del día es obligatorio.")                                           
                .MaximumLength(100).WithMessage("El país no puede superar los 100 caracteres.");                     
                                                                                                                     
            RuleFor(x => x.LocationCity)                                                                             
                .NotEmpty().WithMessage("La ciudad del día es obligatoria.")                                         
                .MaximumLength(100).WithMessage("La ciudad no puede superar los 100 caracteres.");                   
                                                                                                                     
            RuleFor(x => x.TemperatureC)                                                                             
                .InclusiveBetween(-60m, 60m)                                                                         
                .When(x => x.TemperatureC.HasValue)                                                                  
                .WithMessage("La temperatura debe estar en un rango realista entre -60°C y 60°C.");                  
                                                                                                                     
            RuleFor(x => x.Notes)                                                                                    
                .MaximumLength(2000).WithMessage("Las notas del día no pueden superar los 2000 caracteres.");        
        }                                                                                                      
}
