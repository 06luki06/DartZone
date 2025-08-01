using System;
using At.luki0606.DartZone.API.Models;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Models
{
    [TestFixture]
    public class UserTest
    {
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _user = new User("UserName", [1, 2], [3, 4]);
        }

        [Test]
        public void Ctor_PropertiesShallBeInitialized()
        {
            _user.Username.Should().Be("UserName");
            _user.Id.Should().NotBeEmpty().And.NotBe(Guid.Empty);
            _user.PasswordHash.Should().BeEquivalentTo(new byte[] { 1, 2 });
            _user.PasswordSalt.Should().BeEquivalentTo(new byte[] { 3, 4 });
        }
    }
}
