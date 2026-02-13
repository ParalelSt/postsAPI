namespace PostsAPI.DTO.Post.Response;

public class CreatePostResponseDto
{
    public required string Author { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}