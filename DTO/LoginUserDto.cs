using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO;

public class LoginUserDto
{
    [EmailAddress]
    public string? Email { get; set; } = string.Empty;
    
    public string? Password { get; set; } = string.Empty;
}