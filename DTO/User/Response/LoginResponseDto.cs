namespace PostsAPI.DTO.User.Response;

public class LoginResponseDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }

    public List<string> Roles { get; set; } = new();
}