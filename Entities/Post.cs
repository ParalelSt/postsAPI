using System.ComponentModel.DataAnnotations;

namespace PostsAPI.Entities;

public class Post
{
    public required string PostId { get; set; }
    
    public required string UserId { get; set; }
    
    [MaxLength(40)]
    public required string Author { get; set; }
 
    [MaxLength(40)]
    public required string Title { get; set; }
    
    [MaxLength(256)]
    public required string Content { get; set; }
}