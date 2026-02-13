using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.User.Request;

public class RegisterUserDto
{
    [MaxLength(20)]
    public required string UserName { get; set; }
    
    [MaxLength(50)]
    [DefaultValue("")]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    [DefaultValue("")]
    public string? LastName { get; set; }
    
    [MaxLength(254)]
    [EmailAddress]
    public required string Email { get; set; }
    
    [MinLength(8)]
    [MaxLength(256)]
    public required string Password { get; set; }
}