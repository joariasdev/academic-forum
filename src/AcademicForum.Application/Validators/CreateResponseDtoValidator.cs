using AcademicForum.Application.DTOs;
using FluentValidation;

namespace AcademicForum.Application.Validators;

public class CreateResponseDtoValidator : AbstractValidator<CreateResponseDto>
{
    public CreateResponseDtoValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(x => x.DiscussionId)
            .GreaterThan(0).WithMessage("Valid DiscussionId is required.");

        RuleFor(x => x.MemberId)
            .GreaterThan(0).WithMessage("Valid MemberId is required.");
    }
}