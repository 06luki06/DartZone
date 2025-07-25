namespace At.luki0606.DartZone.API.Dtos
{
    public class UserDto
    {
        public readonly string Username;
        public readonly string Password;

        public UserDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
