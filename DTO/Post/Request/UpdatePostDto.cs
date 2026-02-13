using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PostsAPI.DTO.Post.Request;

public class UpdatePostDto
{

    [MaxLength(40)] 
    [DefaultValue("")]
    public string? Title { get; set; }

    [MaxLength(256)]
    [DefaultValue("")]
    public string? Content { get; set; }
}