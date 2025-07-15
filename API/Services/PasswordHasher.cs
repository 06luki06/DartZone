using System.Security.Cryptography;
using System.Text;

namespace At.luki0606.DartZone.API.Services
{
    public static class PasswordHasher
    {
        public static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using HMACSHA512 hmac = new();
            byte[] salt = hmac.Key;
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (hash, salt);
        }
    }
}
