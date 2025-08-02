using System;
using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Mappers.Concrete
{
    [TestFixture]
    public class GameResponseDtoMapperTest
    {
        private GameResponseDtoMapper _dtoMapper;

        [SetUp]
        public void SetUp()
        {
            _dtoMapper = new GameResponseDtoMapper(
                new ThrowResponseDtoMapper(
                    new DartResponseDtoMapper()
                ));
        }

        [Test]
        public void Map_ShouldReturnGameResponseDto_WhenGameIsValid()
        {
            Game game = new(Guid.NewGuid(), 301);

            GameResponseDto result = _dtoMapper.Map(game);

            result.Id.Should().Be(game.Id);
            result.CurrentScore.Should().Be(301);
            result.StartScore.Should().Be(301);
            result.HasFinished.Should().BeFalse();
            result.HasStarted.Should().BeFalse();
            result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            result.Throws.Should().BeEmpty();
        }
    }
}
