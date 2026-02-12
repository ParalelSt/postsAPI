using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO;

public class CreatePostDto
{
    [MaxLength(30)]
    public string? Title { get; set; } = string.Empty;
    
    public string? Content { get; set; } = string.Empty;
}