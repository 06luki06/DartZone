using System;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Enums;
using At.luki0606.DartZone.Shared.Results;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Models
{
    [TestFixture]
    public class ThrowTest
    {
        private Throw _throw;

        [SetUp]
        public void SetUp()
        {
            Dart d1 = new(Multiplier.Single, 10);
            Dart d2 = new(Multiplier.Double, 20);
            Dart d3 = new(Multiplier.Triple, 15);

            _throw = new(Guid.NewGuid(), d1, d2, d3);
        }

        [Test]
        public void Ctor_ShallSetProperties()
        {
            _throw.Id.Should().NotBe(Guid.Empty);
            _throw.GameId.Should().NotBe(Guid.Empty);
            _throw.Game.Should().Be(null);
            _throw.Darts.Should().HaveCount(3);
            _throw.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            _throw.TotalScore.Should().Be(95);
        }

        [Test]
        public void CalculateTotalScore_ShallReturnCorrectScore()
        {
            int score = Throw.CalculateTotalScore(_throw.Darts);
            score.Should().Be(95);
        }

        [Test]
        public void CalculateTotalScore_ShallReturnFailure_WhenDartsCountIsNotThree()
        {
            Result<int> result = Throw.CalculateTotalScore([new(Multiplier.Single, 10)]);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void CalculateTotalScore_ShallReturnFailure_WhenDartsIsNull()
        {
            Result<int> result = Throw.CalculateTotalScore(null);
            result.IsSuccess.Should().BeFalse();
        }
    }
}
