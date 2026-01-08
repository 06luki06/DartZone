using System.Security.Cryptography;
using At.luki0606.DartZone.Shared.Results;

namespace At.luki0606.DartZone.API.Services;

internal static class PasswordHasherService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public static (byte[] hash, byte[] salt) HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA512, HashSize);

        return (hash, salt);
    }

    public static Result VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(password, storedSalt, Iterations, HashAlgorithmName.SHA512, HashSize);

        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash)
            ? Result.Success()
            : Result.Failure("Password is incorrect.");
    }
}
