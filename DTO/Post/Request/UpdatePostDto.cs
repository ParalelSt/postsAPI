using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.Post.Request;

public class UpdatePostDto
{

    [MaxLength(40)] 
    public string? Title { get; set; }

    [MaxLength(256)]
    public string? Content { get; set; }
}