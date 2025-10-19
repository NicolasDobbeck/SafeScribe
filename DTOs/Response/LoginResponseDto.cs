namespace SafeScribe.DTOs.Response
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public LoginResponseDto(string token)
        {
            Token = token;
        }
    }
}