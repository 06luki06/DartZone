using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Requests
{
    [TestFixture]
    public class DartRequestDtoTest
    {
        private DartRequestDto _dartRequest;

        [SetUp]
        public void SetUp()
        {
            _dartRequest = new DartRequestDto();
        }

        [Test]
        public void Ctor_ShouldInitializeProperties()
        {
            _dartRequest.Multiplier.Should().Be(Multiplier.Single);
            _dartRequest.Field.Should().Be(0);
        }

        [Test]
        public void Multiplier_ShouldSetValue()
        {
            _dartRequest.Multiplier = (Multiplier)2;
            _dartRequest.Multiplier.Should().Be(Multiplier.Double);
        }

        [Test]
        public void Field_ShouldSetValue()
        {
            _dartRequest.Field = 20;
            _dartRequest.Field.Should().Be(20);
        }
    }
}
