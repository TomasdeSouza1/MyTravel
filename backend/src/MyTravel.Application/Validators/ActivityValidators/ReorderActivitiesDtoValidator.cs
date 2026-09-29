using System;
using FluentValidation;                                                                                          
using MyTravel.Application.DTOs.Activities;

namespace MyTravel.Application.Validators.ActivityValidators;

public class ReorderActivitiesDtoValidator : AbstractValidator<ReorderActivitiesDto>
{
    public ReorderActivitiesDtoValidator()                                                                       
        {                                                                                                            
            RuleFor(x => x.Items)                                                                                    
                .NotEmpty().WithMessage("Debe enviar al menos una actividad para reordenar.");                       
                                                                                                                     
            RuleForEach(x => x.Items).ChildRules(item =>                                                             
            {                                                                                                        
                item.RuleFor(i => i.ActivityId).NotEmpty().WithMessage("El ID de la actividad es requerido.");       
                item.RuleFor(i => i.NewOrderIndex).GreaterThanOrEqualTo(0).WithMessage("El índice debe ser mayor o igual a 0.");                                                                                                      
            });                                                                                                      
        } 
}
