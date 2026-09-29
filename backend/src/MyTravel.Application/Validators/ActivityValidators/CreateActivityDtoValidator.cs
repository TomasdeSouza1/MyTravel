using System;
using FluentValidation;                                                                                          
using MyTravel.Application.DTOs.Activities;  

namespace MyTravel.Application.Validators.ActivityValidators;

public class CreateActivityDtoValidator : AbstractValidator<CreateActivityDto>
{
    public CreateActivityDtoValidator()                                                                          
        {                                                                                                            
            RuleFor(x => x.Name)                                                                                     
                .NotEmpty().WithMessage("El nombre de la actividad es obligatorio.")                                 
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")                         
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.");                   
                                                                                                                     
            RuleFor(x => x.Category)                                                                                 
                .IsInEnum().WithMessage("La categoría seleccionada no es válida.");                                  
                                                                                                                     
            RuleFor(x => x.Latitude)                                                                                 
                .InclusiveBetween(-90.0, 90.0).WithMessage("La latitud debe estar entre -90 y 90 grados.");          
                                                                                                                     
            RuleFor(x => x.Longitude)                                                                                
                .InclusiveBetween(-180.0, 180.0).WithMessage("La longitud debe estar entre -180 y 180 grados.");     
                                                                                                                     
            RuleFor(x => x.EndTime)                                                                                  
                .GreaterThanOrEqualTo(x => x.StartTime)                                                              
                .WithMessage("La hora de fin no puede ser anterior a la hora de inicio.");                           
                                                                                                                     
            RuleFor(x => x.OrderIndex)                                                                               
                .GreaterThanOrEqualTo(0).WithMessage("El índice de orden debe ser mayor o igual a 0.");              
                                                                                                                     
            RuleFor(x => x.BookingReference)                                                                         
                .MaximumLength(50).WithMessage("El código de reserva no puede superar los 50 caracteres.");          
                                                                                                                     
            RuleFor(x => x.Notes)                                                                                    
                .MaximumLength(1000).WithMessage("Las notas de la actividad no pueden superar los 1000 caracteres.");
        }                          
}
