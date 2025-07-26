using FluentValidation;

namespace At.luki0606.DartZone.API.Validators
{
    public interface IValidatorFactory
    {
        AbstractValidator<TEntity> GetValidator<TEntity>()
            where TEntity : class;
    }
}
