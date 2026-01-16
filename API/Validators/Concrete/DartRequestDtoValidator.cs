using At.luki0606.DartZone.Shared.Dtos.Requests;
using FluentValidation;

namespace At.luki0606.DartZone.API.Validators.Concrete;

internal sealed class DartRequestDtoValidator : AbstractValidator<DartRequestDto>
{
    public DartRequestDtoValidator()
    {
        RuleFor(d => d.Multiplier)
            .IsInEnum()
            .WithMessage("Multiplier must be a valid enum value.");
        RuleFor(d => d.Field)
            .Must(field => field is 25 or (>= 0 and <= 20))
            .WithMessage("Field must be 25 or between 1 and 20.");
    }
}
