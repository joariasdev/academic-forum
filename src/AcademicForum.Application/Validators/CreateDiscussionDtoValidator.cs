using AcademicForum.Application.DTOs;
using FluentValidation;

namespace AcademicForum.Application.Validators;

public class CreateDiscussionDtoValidator : AbstractValidator<CreateDiscussionDto>
{
    public CreateDiscussionDtoValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(x => x.MovieId)
            .GreaterThan(0).WithMessage("Valid MovieId is required.");

        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("Valid EventId is required.");

        RuleFor(x => x.MemberId)
            .GreaterThan(0).WithMessage("Valid MemberId is required.");
    }
}