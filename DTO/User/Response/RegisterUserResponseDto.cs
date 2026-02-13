namespace PostsAPI.DTO.User.Response;

public class RegisterUserResponseDto
{
    public string? Message { get; set; }
    public required UserResponseDto User { get; set; }
}