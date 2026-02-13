namespace PostsAPI.DTO.Post.Response;

public class CurrentUserPostResponseDto
{
    public required string PostId { get; set; }
    public required string Author { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}