using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.Post.Request;

public class CreatePostDto
{
    [MaxLength(40)]
    [DefaultValue("")]
    public required string Title { get; set; }
    
    [MaxLength(256)]
    [DefaultValue("")]
    public required string Content { get; set; }
}