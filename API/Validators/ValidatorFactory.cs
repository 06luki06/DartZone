using System;
using At.luki0606.DartZone.Shared.Results;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace At.luki0606.DartZone.API.Validators
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Result<IValidator<TEntity>> GetValidator<TEntity>() where TEntity : class
        {
            IValidator<TEntity> validator = _serviceProvider.GetService<IValidator<TEntity>>();
            return validator != null
                ? Result<IValidator<TEntity>>.Success(validator)
                : Result<IValidator<TEntity>>.Failure($"No validator found for type {typeof(TEntity).Name}");
        }
    }
}
