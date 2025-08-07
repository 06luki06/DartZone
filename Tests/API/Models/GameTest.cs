using System;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Results;
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

        [Test]
        public void AddThrow_GameHasBeenFinished_ShouldReturnFailure()
        {
            _game.HasFinished = true;
            Result result = _game.AddThrow(HelperMethods.GetSampleThrow(_game.Id));
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void AddThrow_ThrowObjIsNull_ShouldReturnFailure()
        {
            Result result = _game.AddThrow(null);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void AddThrow_ThrowDoesNotBelongToGame_ShouldReturnFailure()
        {
            Result result = _game.AddThrow(HelperMethods.GetSampleThrow(Guid.NewGuid()));
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void AddThrow_ThrowAlreadyAdded_ShouldReturnFailure()
        {
            Throw @throw = HelperMethods.GetSampleThrow(_game.Id);
            _game.AddThrow(@throw);
            Result result = _game.AddThrow(@throw);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void AddThrow_ShouldAddThrow()
        {
            _game.HasStarted.Should().BeFalse();

            Throw @throw = HelperMethods.GetSampleThrow(_game.Id);
            Result result = _game.AddThrow(@throw);
            result.IsSuccess.Should().BeTrue();
            _game.Throws.Should().Contain(@throw);
            _game.HasStarted.Should().BeTrue();
        }

        [Test]
        public void AddThrow_ThrowOverthrowsRemainingScore_ShouldReturnFailure()
        {
            Game game = new(Guid.NewGuid(), 10);

            Throw @throw = HelperMethods.GetSampleThrow(game.Id);
            Result result = game.AddThrow(@throw);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void AddThrow_ThrowFinishesGame()
        {
            Game game = new(Guid.NewGuid(), 65);

            Throw @throw = HelperMethods.GetSampleThrow(game.Id);
            Result result = game.AddThrow(@throw);
            result.IsSuccess.Should().BeTrue();
            game.CurrentScore.Should().Be(0);
            game.HasFinished.Should().BeTrue();
        }
    }
}
