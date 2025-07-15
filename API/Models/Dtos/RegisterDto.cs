namespace At.luki0606.DartZone.API.Models.Dtos
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisterDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
