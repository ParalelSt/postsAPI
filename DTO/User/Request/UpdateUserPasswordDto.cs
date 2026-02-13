using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.User.Request;

public class UpdateUserPasswordDto
{   
    [MinLength(8)]
    [MaxLength(256)]
    public required string CurrentPassword { get; set; }
    
    [MinLength(8)]
    [MaxLength(256)]
    public required string NewPassword { get; set; }
}