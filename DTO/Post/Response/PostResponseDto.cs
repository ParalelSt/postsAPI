namespace PostsAPI.DTO.Post.Response;

public class PostResponseDto
{
    public required string Author { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}