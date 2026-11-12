using System;
using FluentValidation;
using MyTravel.Application.DTOs.Collaboration;

namespace MyTravel.Application.Validators.CollaborationValidators;

public class JoinTripValidator : AbstractValidator<JointripDto>
{
    public JoinTripValidator()
    {
        RuleFor(x => x.InviteToken)
            .NotEmpty().WithMessage("Invite token es requerido.")
            .MaximumLength(64).WithMessage("Invite token no puede exceder 64 caracteres.")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Invite token solo puede contener caracteres alfanuméricos.");
    }   
}
