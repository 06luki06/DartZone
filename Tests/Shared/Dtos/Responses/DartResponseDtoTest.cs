using System;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Responses;

[TestFixture]
internal sealed class DartResponseDtoTest
{
    private DartResponseDto _dartResponseDto;

    [SetUp]
    public void SetUp()
    {
        _dartResponseDto = new DartResponseDto();
    }

    [Test]
    public void Ctor_ShallHaveDefaultValues()
    {
        _dartResponseDto.Id.Should().Be(Guid.Empty);
        _dartResponseDto.Multiplier.Should().Be(Multiplier.Single);
        _dartResponseDto.Field.Should().Be(0);
        _dartResponseDto.Score.Should().Be(0);
    }

    [TestCase(Multiplier.Single, 5)]
    [TestCase(Multiplier.Double, 10)]
    [TestCase(Multiplier.Triple, 15)]
    public void Score_ShallBeCalculatedCorrectly(Multiplier multiplier, int expectedResult)
    {
        _dartResponseDto.Field = 5;
        _dartResponseDto.Multiplier = multiplier;
        _dartResponseDto.Score.Should().Be(expectedResult);
    }
}
