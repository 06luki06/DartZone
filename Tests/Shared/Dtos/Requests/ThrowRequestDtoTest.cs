using At.luki0606.DartZone.Shared.Dtos.Requests;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Requests
{
    [TestFixture]
    public class ThrowRequestDtoTest
    {
        private ThrowRequestDto _throwRequestDto;

        [SetUp]
        public void SetUp()
        {
            _throwRequestDto = new();
        }

        [Test]
        public void Ctor_ShouldInitializeProperties()
        {
            _throwRequestDto.Dart1.Should().NotBeNull();
            _throwRequestDto.Dart2.Should().NotBeNull();
            _throwRequestDto.Dart3.Should().NotBeNull();
        }
    }
}
