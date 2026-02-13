namespace PostsAPI.DTO;

public class UserResponseDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Roles { get; set; }
}