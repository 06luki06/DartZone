using System;
using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Mappers.Concrete
{
    [TestFixture]
    public class ThrowResponseDtoMapperTest
    {
        private ThrowResponseDtoMapper _dtoMapper;

        [SetUp]
        public void SetUp()
        {
            _dtoMapper = new ThrowResponseDtoMapper(new DartResponseDtoMapper());
        }

        [Test]
        public void Map_ShouldReturnThrowResponseDto_WhenThrowIsValid()
        {
            Dart d1 = new(Multiplier.Double, 5);
            Dart d2 = new(Multiplier.Single, 10);
            Dart d3 = new(Multiplier.Triple, 15);

            Throw @throw = new(Guid.NewGuid(), d1, d2, d3);
            ThrowResponseDto result = _dtoMapper.Map(@throw);

            result.TotalScore.Should().Be(65);
            result.Id.Should().Be(@throw.Id);
            result.Darts.Should().HaveCount(3);
        }
    }
}
