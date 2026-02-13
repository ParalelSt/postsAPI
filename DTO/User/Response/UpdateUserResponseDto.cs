namespace PostsAPI.DTO.User.Response;

public class UpdateUserResponseDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Initials { get; set; }
    
}