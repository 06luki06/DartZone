using System;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Dtos.Responses;

[TestFixture]
internal sealed class UserResponseDtoTest
{
    private UserResponseDto _userResponseDto;

    [SetUp]
    public void SetUp()
    {
        _userResponseDto = new();
    }

    [Test]
    public void Ctor_ShouldInitializeProperties()
    {
        _userResponseDto.Username.Should().BeEmpty();
        _userResponseDto.Id.Should().Be(Guid.Empty);
    }
}
