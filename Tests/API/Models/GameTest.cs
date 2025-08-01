using System;
using At.luki0606.DartZone.API.Models;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Models
{
    [TestFixture]
    public class GameTest
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game(Guid.NewGuid(), 501);
        }

        [Test]
        public void Ctor_ShouldInitializePropertiesCorrectly()
        {
            _game.Id.Should().NotBe(Guid.Empty);
            _game.UserId.Should().NotBe(Guid.Empty);
            _game.User.Should().BeNull();
            _game.StartScore.Should().Be(501);
            _game.CurrentScore.Should().Be(501);
            _game.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            _game.HasFinished.Should().BeFalse();
            _game.HasStarted.Should().BeFalse();
            _game.Throws.Should().BeEmpty();
        }
    }
}
