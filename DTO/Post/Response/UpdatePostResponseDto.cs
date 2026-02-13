namespace PostsAPI.DTO.Post.Response;

public class UpdatePostResponseDto
{
    public string? Message { get; set; }
    public required PostResponseDto Post { get; set; }
}