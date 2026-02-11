using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO;

public class RegisterUserDto
{
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
    
    [MaxLength(30)]
    public string? Email { get; set; } = string.Empty;
    
    public string? Password { get; set; } = string.Empty;
}