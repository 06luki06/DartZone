using System;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Results;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace At.luki0606.DartZone.Tests.API.Mappers;

[TestFixture]
internal sealed class DtoMapperFactoryTest
{
    private DtoMapperFactory _mapperFactory;
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void SetUp()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IDtoMapper<Dart, DartResponseDto>, DartResponseDtoMapper>();
        services.AddScoped<IDtoMapperFactory, DtoMapperFactory>();
        _serviceProvider = services.BuildServiceProvider();
        _mapperFactory = new DtoMapperFactory(_serviceProvider);
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
        FluentActions.Invoking(() => new DtoMapperFactory(null))
            .Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetValidator_ValidType_ShallReturnValidator()
    {
        Result<IDtoMapper<Dart, DartResponseDto>> validatorResult = _mapperFactory.GetMapper<Dart, DartResponseDto>();
        validatorResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void GetValidator_InvalidType_ShallThrowInvalidOperationException()
    {
        Result<IDtoMapper<string, DartResponseDto>> validator = _mapperFactory.GetMapper<string, DartResponseDto>();
        validator.IsFailure.Should().BeTrue();
    }
}
