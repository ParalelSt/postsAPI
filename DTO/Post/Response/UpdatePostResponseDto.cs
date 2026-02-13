using PostsAPI.Entities;

namespace PostsAPI.DTO;

public class UpdatePostResponseDto
{
    public string? Message { get; set; }
    public Post? Post { get; set; }
}