namespace PostsAPI.DTO.User.Response;

public class UserResponseDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required DateTime CreatedAt { get; set; }
    public List<string> Roles { get; set; } = new();
}