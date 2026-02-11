using System.ComponentModel.DataAnnotations;

namespace PostsAPI.Entities;

public class Post
{
    public string? PostID { get; set; }
    
    public string? UserID { get; set; }
 
    [MaxLength(30)]
    public string? Title { get; set; }
    
    public string? Content { get; set; }
}