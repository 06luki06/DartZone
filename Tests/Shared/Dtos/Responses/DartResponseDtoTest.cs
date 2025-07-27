using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Responses
{
    [TestFixture]
    public class DartResponseDtoTest
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
            _dartResponseDto.Id.Should().NotBeEmpty();
            _dartResponseDto.Multiplier.Should().Be(Multiplier.Single);
            _dartResponseDto.Field.Should().Be(0);
            _dartResponseDto.Score.Should().Be(0);
        }

        [Test]
        public void Score_ShallBeCalculatedCorrectly()
        {
            _dartResponseDto.Field = 5;
            _dartResponseDto.Multiplier = Multiplier.Single;
            _dartResponseDto.Score.Should().Be(5);
            _dartResponseDto.Multiplier = Multiplier.Double;
            _dartResponseDto.Score.Should().Be(10);
            _dartResponseDto.Multiplier = Multiplier.Triple;
            _dartResponseDto.Score.Should().Be(15);
        }
    }
}
