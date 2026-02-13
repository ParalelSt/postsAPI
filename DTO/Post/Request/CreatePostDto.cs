using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.Post.Request;

public class CreatePostDto
{
    [MaxLength(40)]
    public required string Title { get; set; }
    
    [MaxLength(256)]
    public required string Content { get; set; }
}