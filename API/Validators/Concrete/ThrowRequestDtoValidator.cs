using At.luki0606.DartZone.Shared.Dtos.Requests;
using FluentValidation;

namespace At.luki0606.DartZone.API.Validators.Concrete;

internal class ThrowRequestDtoValidator : AbstractValidator<ThrowRequestDto>
{
    public ThrowRequestDtoValidator()
    {
        RuleFor(t => t.Dart1)
            .NotNull().WithMessage("Dart1 is required.")
            .SetValidator(new DartRequestDtoValidator());

        RuleFor(t => t.Dart2)
            .NotNull().WithMessage("Dart2 is required.")
            .SetValidator(new DartRequestDtoValidator());

        RuleFor(t => t.Dart3)
            .NotNull().WithMessage("Dart3 is required.")
            .SetValidator(new DartRequestDtoValidator());
    }
}
