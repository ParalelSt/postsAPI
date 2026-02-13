using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO;

public class RegisterUserDto
{
    public string? UserName { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? FirstName { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? LastName { get; set; } = string.Empty;
    
    [EmailAddress]
    [MaxLength(30)]
    public string? Email { get; set; } = string.Empty;
    
    public string? Password { get; set; } = string.Empty;
}