using AcademicForum.Application.DTOs;
using FluentValidation;

namespace AcademicForum.Application.Validators;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Event date cannot be in the past.");

        RuleFor(x => x.MovieId)
            .GreaterThan(0).WithMessage("Valid MovieId is required.");

        RuleFor(x => x.VenueId)
            .GreaterThan(0).WithMessage("Valid VenueId is required.");
    }
}