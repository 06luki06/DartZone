using System;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Models
{
    [TestFixture]
    public class DartTest
    {
        private Dart _dart;

        [SetUp]
        public void SetUp()
        {
            _dart = new Dart(Multiplier.Single, 10);
        }

        [Test]
        public void Ctor_PropertiesShallBeInitialized()
        {
            _dart.Id.Should().NotBeEmpty().And.NotBe(Guid.Empty);
            _dart.ThrowId.Should().Be(Guid.Empty);
            _dart.Throw.Should().BeNull();
            _dart.Multiplier.Should().Be(Multiplier.Single);
            _dart.Field.Should().Be(10);
            _dart.Score.Should().Be(10);
        }

        [TestCase(Multiplier.Single, 0, 0)]
        [TestCase(Multiplier.Triple, -3, 0)]
        [TestCase(Multiplier.Single, 1, 1)]
        [TestCase(Multiplier.Single, 21, 0)]
        [TestCase(Multiplier.Single, 20, 20)]
        [TestCase(Multiplier.Triple, 18, 54)]
        [TestCase(Multiplier.Double, 19, 38)]
        [TestCase(Multiplier.Triple, 25, 0)]
        [TestCase(Multiplier.Double, 25, 50)]
        public void CalculateScore_ShallReturnCorrectScore(Multiplier multiplier, int field, int expectedResult)
        {
            int actualResult = Dart.CalculateScore(multiplier, field);
            actualResult.Should().Be(expectedResult);
        }

        [Test]
        public void SetThrow_ShouldSetThrowProperties()
        {
            Throw throwObj = new(Guid.NewGuid(), new Dart(Multiplier.Single, 10), new Dart(Multiplier.Double, 20), new Dart(Multiplier.Triple, 15));
            _dart.SetThrow(throwObj);
            _dart.Throw.Should().Be(throwObj);
            _dart.ThrowId.Should().Be(throwObj.Id);
        }

        [Test]
        public void SetThrow_ThrowObjIsNull_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => _dart.SetThrow(null))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void SetThrow_ThrowIsAlreadySet_ShouldThrowInvalidOperationException()
        {
            Throw throwObj = new(Guid.NewGuid(), _dart, new Dart(Multiplier.Double, 20), new Dart(Multiplier.Triple, 15));

            FluentActions.Invoking(() => _dart.SetThrow(throwObj))
                .Should().Throw<InvalidOperationException>();
        }
    }
}
