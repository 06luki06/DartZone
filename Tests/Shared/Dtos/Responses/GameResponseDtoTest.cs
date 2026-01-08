using System;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Responses;

[TestFixture]
internal sealed class GameResponseDtoTest
{
    private GameResponseDto _gameResponseDto;

    [SetUp]
    public void SetUp()
    {
        _gameResponseDto = new GameResponseDto();
    }

    [Test]
    public void Ctor_ShouldInitializeProperties()
    {
        _gameResponseDto.Id.Should().Be(Guid.Empty);
        _gameResponseDto.StartScore.Should().Be(0);
        _gameResponseDto.CurrentScore.Should().Be(0);
        _gameResponseDto.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        _gameResponseDto.HasFinished.Should().BeFalse();
        _gameResponseDto.HasStarted.Should().BeFalse();
        _gameResponseDto.Throws.Should().BeEmpty();
    }
}
