using PostsAPI.DTO.Post.Request;
using PostsAPI.DTO.Post.Response;
using PostsAPI.Entities;

namespace PostsAPI.Services;

public interface IPostService
{
    Task<List<PostResponseDto>> GetAllPosts();
    Task<CreatePostResponseDto> CreatePost(string userName, string userId, CreatePostDto dto);
    Task<List<PostResponseDto>> GetPostsFromCurrentUser(string userId);
    Task<UpdatePostResponseDto> UpdatePost(string userId, string postId, UpdatePostDto dto);
    Task<string> DeletePost(string userId, string postId);
}