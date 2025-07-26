using System;
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

        public AbstractValidator<TEntity> GetValidator<TEntity>() where TEntity : class
        {
            AbstractValidator<TEntity> validator = _serviceProvider.GetService<AbstractValidator<TEntity>>();
            return validator ?? throw new InvalidOperationException($"No validator registered for {typeof(TEntity).Name}");
        }
    }
}
