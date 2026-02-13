using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.User.Request;

public class UpdateUserDto
{
    [MaxLength(20)]
    public string? UserName { get; set; }
    
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
    
    [MaxLength(254)]
    [EmailAddress]
    public string? Email { get; set; }
}