using System;

namespace At.luki0606.DartZone.API.Models
{
    public class User
    {
        #region Properties
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        #endregion

        #region Ctor
        private User() { }

        public User(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
        #endregion
    }
}
