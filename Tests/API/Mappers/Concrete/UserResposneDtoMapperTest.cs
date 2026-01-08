using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Mappers.Concrete;

[TestFixture]
internal sealed class UserResposneDtoMapperTest
{
    private UserResponseDtoMapper _dtoMapper;

    [SetUp]
    public void SetUp()
    {
        _dtoMapper = new UserResponseDtoMapper();
    }

    [Test]
    public void Map_ShouldReturnUserResponseDto_WhenUserIsValid()
    {
        User user = new("TestUser", new byte[3], new byte[3]);
        UserResponseDto result = _dtoMapper.Map(user);

        result.Username.Should().Be("TestUser");
        result.Id.Should().Be(user.Id);
    }
}
