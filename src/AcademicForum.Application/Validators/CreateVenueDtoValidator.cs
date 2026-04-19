using AcademicForum.Application.DTOs;
using FluentValidation;

namespace AcademicForum.Application.Validators;

public class CreateVenueDtoValidator : AbstractValidator<CreateVenueDto>
{
    public CreateVenueDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than zero.");
    }
}