using System;
using FluentValidation;
using MyTravel.Application.DTOs.Trips;

namespace MyTravel.Application.Validators;

public class UpdateTripDtoValidator : AbstractValidator<UpdateTripDto>
{
public UpdateTripDtoValidator()                                                                              
        {                                                                                                            
            RuleFor(x => x.Title)                                                                                    
                .NotEmpty().WithMessage("El título del viaje es obligatorio.")                                       
                .MinimumLength(3).WithMessage("El título debe tener al menos 3 caracteres.")                         
                .MaximumLength(120).WithMessage("El título no puede superar los 120 caracteres.");                   
                                                                                                                     
            RuleFor(x => x.DestinationCountry)                                                                       
                .NotEmpty().WithMessage("El país de destino es obligatorio.")                                        
                .MaximumLength(100).WithMessage("El país no puede superar los 100 caracteres.");                     
                                                                                                                     
            RuleFor(x => x.DestinationCity)                                                                          
                .NotEmpty().WithMessage("La ciudad de destino es obligatoria.")                                      
                .MaximumLength(100).WithMessage("La ciudad no puede superar los 100 caracteres.");                   
                                                                                                                     
            RuleFor(x => x.BaseCurrency)                                                                             
                .NotEmpty().WithMessage("La moneda base es obligatoria.")                                            
                .Matches(@"^[A-Z]{3}$").WithMessage("La moneda debe ser un código ISO de 3 letras mayúsculas (ej. USD,EUR, ARS).");                                                                                                      
                                                                                                                     
            RuleFor(x => x.TotalBudget)                                                                              
                .GreaterThanOrEqualTo(0).WithMessage("El presupuesto no puede ser negativo.")                        
                .LessThanOrEqualTo(100_000_000).WithMessage("El presupuesto excede el límite máximo permitido.");    
                                                                                                                     
            RuleFor(x => x.CoverImageUrl)                                                                            
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))                                            
                .When(x => !string.IsNullOrWhiteSpace(x.CoverImageUrl))                                              
                .WithMessage("La URL de la imagen de portada debe ser una URL válida (http/https).");                
                                                                                                                     
            RuleFor(x => x.StartDate)                                                                                
                .NotEmpty().WithMessage("La fecha de inicio es requerida.");                                         
                                                                                                                     
            RuleFor(x => x.EndDate)                                                                                  
                .NotEmpty().WithMessage("La fecha de fin es requerida.")                                             
                .GreaterThanOrEqualTo(x => x.StartDate)                                                              
                .WithMessage("La fecha de fin no puede ser anterior a la fecha de inicio.")                          
                .Must((dto, end) => (end.DayNumber - dto.StartDate.DayNumber) <= 50)                                 
                .WithMessage("La duración del viaje no puede superar los 50 días.");                                 
        }   
}
