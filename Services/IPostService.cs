using PostsAPI.DTO;
using PostsAPI.Entities;

namespace PostsAPI.Services;

public interface IPostService
{
    Task<List<PostResponseDto>> GetAllPosts();
    Task<CreatePostResponseDto> CreatePost(string userId, CreatePostDto dto);
    Task<List<Post>> GetPostsFromCurrentUser(string userId);
    Task<UpdatePostResponseDto> UpdatePost(string userId, string postId, UpdatePostDto dto);
    Task<string> DeletePost(string userId, string postId);
}