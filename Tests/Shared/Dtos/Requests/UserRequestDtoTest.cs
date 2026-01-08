using At.luki0606.DartZone.Shared.Dtos.Requests;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Requests;

[TestFixture]
internal sealed class UserRequestDtoTest
{
    private UserRequestDto _userRequestDto;

    [SetUp]
    public void SetUp()
    {
        _userRequestDto = new();
    }

    [Test]
    public void Ctor_ShallHaveDefaultValues()
    {
        _userRequestDto.Username.Should().BeEmpty();
        _userRequestDto.Password.Should().BeEmpty();
    }

    [Test]
    public void Username_ShouldBeSettable()
    {
        const string username = "testuser";
        _userRequestDto.Username = username;
        _userRequestDto.Username.Should().Be(username);
    }

    [Test]
    public void Password_ShouldBeSettable()
    {
        const string password = "testpassword";
        _userRequestDto.Password = password;
        _userRequestDto.Password.Should().Be(password);
    }
}
