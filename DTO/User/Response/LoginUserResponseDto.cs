namespace PostsAPI.DTO.User.Response;

public class LoginUserResponseDto
{
    public string? Message { get; set; }
    public required LoginResponseDto User { get; set; }
}