using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PostsAPI.Entities;

public class User: IdentityUser
{
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
    
    [MaxLength(5)]
    public string? Initials { get; set; }
    
    public DateTime CreatedAt { get; set; }
}