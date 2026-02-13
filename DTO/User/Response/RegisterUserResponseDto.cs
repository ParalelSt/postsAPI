namespace PostsAPI.DTO;

public class RegisterUserResponseDto
{
    public string? Message { get; set; }
    public UserResponseDto User { get; set; }
}