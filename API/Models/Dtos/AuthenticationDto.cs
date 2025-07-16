namespace At.luki0606.DartZone.API.Models.Dtos
{
    public class AuthenticationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthenticationDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
