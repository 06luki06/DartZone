using System;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Responses
{
    [TestFixture]
    public class ThrowResponseDtoTest
    {
        private ThrowResponseDto _throwResponse;

        [SetUp]
        public void SetUp()
        {
            _throwResponse = new ThrowResponseDto();
        }

        [Test]
        public void Ctor_ShouldInitializeProperties()
        {
            _throwResponse.Id.Should().Be(Guid.Empty);
            _throwResponse.Darts.Should().BeEmpty();
            _throwResponse.TotalScore.Should().Be(0);
        }

        [Test]
        public void AddDart_ShouldAddDartToDartsList()
        {
            DartResponseDto dart1 = new() { Field = 10 };
            DartResponseDto dart2 = new() { Field = 20, Multiplier = Multiplier.Double };
            DartResponseDto dart3 = new() { Field = 5, Multiplier = Multiplier.Triple };
            _throwResponse.Darts = [dart1, dart2, dart3];
            _throwResponse.Darts.Should().HaveCount(3);
            _throwResponse.TotalScore.Should().Be(65);
        }
    }
}
