using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Responses
{
    [TestFixture]
    public class TokenResponseDtoTest
    {
        private TokenResponseDto _tokenResponse;

        [SetUp]
        public void SetUp()
        {
            _tokenResponse = new TokenResponseDto();
        }

        [Test]
        public void Ctor_ShouldInitializeProperties()
        {
            _tokenResponse.Token.Should().BeEmpty();
        }

        [Test]
        public void Token_ShouldBeSettable()
        {
            string token = "test";
            _tokenResponse.Token = token;
            _tokenResponse.Token.Should().Be(token);
        }
    }
}
