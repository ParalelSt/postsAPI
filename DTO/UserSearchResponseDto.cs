namespace PostsAPI.DTO;

public class UserSearchResponseDto
{
    public string? UserName { get; set; }
    public List<string> Roles { get; set; }
}