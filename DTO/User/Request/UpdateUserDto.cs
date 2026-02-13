using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.User.Request;

public class UpdateUserDto
{
    [MaxLength(20)]
    [DefaultValue("")]
    public string? UserName { get; set; }
    
    [MaxLength(50)]
    [DefaultValue("")]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    [DefaultValue("")]
    public string? LastName { get; set; }
    
    [MaxLength(254)]
    [DefaultValue(null)]
    [EmailAddress]
    public string? Email { get; set; }
}