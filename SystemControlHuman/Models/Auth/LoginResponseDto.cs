namespace SystemControlHuman.Models.Auth;

public class LoginResponseDto
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
}