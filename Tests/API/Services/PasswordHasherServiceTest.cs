using At.luki0606.DartZone.API.Services;
using At.luki0606.DartZone.Shared.Results;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.API.Services
{
    [TestFixture]
    public class PasswordHasherServiceTest
    {
        [Test]
        public void HashPassword_ShouldReturnDifferentHashes_ForSamePassword()
        {
            const string password = "MySecurePassword123";

            (byte[] hash1, byte[] salt1) = PasswordHasherService.HashPassword(password);
            (byte[] hash2, byte[] salt2) = PasswordHasherService.HashPassword(password);

            hash1.Should().NotBeEquivalentTo(hash2);
            salt1.Should().NotBeEquivalentTo(salt2);
        }

        [Test]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            const string password = "Secure123!";
            (byte[] hash, byte[] salt) = PasswordHasherService.HashPassword(password);
            Result result = PasswordHasherService.VerifyPassword(password, hash, salt);
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            (byte[] hash, byte[] salt) = PasswordHasherService.HashPassword("correct-password");

            Result result = PasswordHasherService.VerifyPassword("wrong-password", hash, salt);
            result.IsFailure.Should().BeTrue();
        }

        [Test]
        public void HashPassword_ShouldGenerateHashAndSalt_OfExpectedLength()
        {
            (byte[] hash, byte[] salt) = PasswordHasherService.HashPassword("test");
            hash.Length.Should().Be(32); // 32 bytes = 256 bit hash
            salt.Length.Should().Be(16); // 16 bytes = 128 bit salt
        }
    }
}
