using System;

namespace At.luki0606.DartZone.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            Username = string.Empty;
            PasswordHash = [];
            PasswordSalt = [];
        }

        public User(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
