using System;
using At.luki0606.DartZone.API.Validators;
using At.luki0606.DartZone.API.Validators.Concrete;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Results;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using IValidatorFactory = At.luki0606.DartZone.API.Validators.IValidatorFactory;

namespace At.luki0606.DartZone.Tests.API.Validators;

[TestFixture]
internal sealed class ValidatorFactoryTest
{
    private ValidatorFactory _validatorFactory;
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void SetUp()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IValidator<UserRequestDto>, UserRequestDtoValidator>();
        services.AddScoped<IValidatorFactory, ValidatorFactory>();
        _serviceProvider = services.BuildServiceProvider();
        _validatorFactory = new ValidatorFactory(_serviceProvider);
    }

    [TearDown]
    public void TearDown()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    [Test]
    public void Ctor_ServiceProviderIsNull_ShallThrowArgumentException()
    {
        FluentActions.Invoking(() => new ValidatorFactory(null))
            .Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetValidator_ValidType_ShallReturnValidator()
    {
        Result<IValidator<UserRequestDto>> validatorResult = _validatorFactory.GetValidator<UserRequestDto>();
        validatorResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void GetValidator_InvalidType_ShallThrowInvalidOperationException()
    {
        Result<IValidator<string>> validator = _validatorFactory.GetValidator<string>();
        validator.IsSuccess.Should().BeFalse();
    }
}
