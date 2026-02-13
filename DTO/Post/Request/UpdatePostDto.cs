using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO;

public class UpdatePostDto
{
 
    [MaxLength(40)]
    public string? Title { get; set; }
    
    public string? Content { get; set; }
}