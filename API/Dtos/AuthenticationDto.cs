namespace At.luki0606.DartZone.API.Dtos
{
    public class AuthenticationDto
    {
        public readonly string Username;
        public readonly string Password;

        public AuthenticationDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
