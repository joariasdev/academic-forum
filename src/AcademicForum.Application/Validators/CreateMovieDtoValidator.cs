using AcademicForum.Application.DTOs;
using FluentValidation;

namespace AcademicForum.Application.Validators;

public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
{
    public CreateMovieDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Sinopsis)
            .NotEmpty().WithMessage("Sinopsis is required.");

        RuleFor(x => x.Director)
            .NotEmpty().WithMessage("Director is required.");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required.");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Release date cannot be in the future.");
    }
}