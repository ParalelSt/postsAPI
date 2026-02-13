using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.User.Request;

public class LoginUserDto
{   
    [MaxLength(254)]
    [EmailAddress]
    public required string Email { get; set; }
    
    [MinLength(8)]
    [MaxLength(256)]
    public required string Password { get; set; }
}