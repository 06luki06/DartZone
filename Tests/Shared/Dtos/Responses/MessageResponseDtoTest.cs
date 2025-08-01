using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Responses
{
    [TestFixture]
    public class MessageResponseDtoTest
    {
        private MessageResponseDto _messageResponseDto;

        [SetUp]
        public void SetUp()
        {
            _messageResponseDto = new MessageResponseDto();
        }

        [Test]
        public void Ctor_ShouldInitializeProperties()
        {
            _messageResponseDto.Message.Should().Be(string.Empty);
        }

        [Test]
        public void Message_ShouldBeSettable()
        {
            string message = "Test message";
            _messageResponseDto.Message = message;
            _messageResponseDto.Message.Should().Be(message);
        }
    }
}
