using At.luki0606.DartZone.Shared.Results;
using FluentValidation;

namespace At.luki0606.DartZone.API.Validators
{
    public interface IValidatorFactory
    {
        Result<IValidator<TEntity>> GetValidator<TEntity>()
            where TEntity : class;
    }
}
