namespace PostsAPI.DTO;

public class LoginUserResponseDto
{
    public string? Message { get; set; }
    public LoginResponseDto User { get; set; }
}