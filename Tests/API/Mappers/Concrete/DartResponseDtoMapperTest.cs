using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Mappers.Concrete;

[TestFixture]
internal sealed class DartResponseDtoMapperTest
{
    private DartResponseDtoMapper _dtoMapper;

    [SetUp]
    public void SetUp()
    {
        _dtoMapper = new DartResponseDtoMapper();
    }

    [Test]
    public void Map_ShouldReturnDartResponseDto_WhenDartIsValid()
    {
        Dart dart = new(Multiplier.Double, 5);
        DartResponseDto result = _dtoMapper.Map(dart);

        result.Multiplier.Should().Be(Multiplier.Double);
        result.Field.Should().Be(5);
        result.Id.Should().Be(dart.Id);
        result.Score.Should().Be(10);
    }
}
