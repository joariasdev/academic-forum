using AcademicForum.Application.DTOs;
using FluentValidation;

namespace AcademicForum.Application.Validators;

public class CreateAttendeeRecordDtoValidator : AbstractValidator<CreateAttendeeRecordDto>
{
    public CreateAttendeeRecordDtoValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("Valid EventId is required.");

        RuleFor(x => x.MemberId)
            .GreaterThan(0).WithMessage("Valid MemberId is required.");
    }
}