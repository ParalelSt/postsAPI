using PostsAPI.DTO;
using PostsAPI.Entities;

namespace PostsAPI.Services;

public interface IPostService
{
    Task<List<List<PostResponseDto>>> GetAllPosts();
    Task<Post> CreatePost(CreatePostDto dto);
    Task<List<Post>> GetPostsFromCurrentUser();
    Task<Post> UpdatePost(UpdatePostDto dto);
    Task<string> DeletePost(string userId);
}