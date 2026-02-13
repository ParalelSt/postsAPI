using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PostsAPI.Data;
using PostsAPI.DTO.Post.Request;
using PostsAPI.DTO.Post.Response;
using PostsAPI.Entities;

namespace PostsAPI.Services;

public class PostService: IPostService
{
    private readonly AppDbContext _dbContext;
    
    public PostService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<PostResponseDto>> GetAllPosts()
    {
        var posts = await _dbContext.Posts.Select(p => new PostResponseDto
        {
            Author = p.Author,
            Title = p.Title,
            Content = p.Content
        }).ToListAsync();
        
        return posts;
    }
    
    public async Task<CreatePostResponseDto> CreatePost(string userName,string userId, CreatePostDto createPost)
    {
        if (string.IsNullOrWhiteSpace(createPost.Content) || string.IsNullOrWhiteSpace(createPost.Title))
        {
            throw new KeyNotFoundException("You must enter the post details");
        }

        var post = new Post
        {   
            PostId = Guid.NewGuid().ToString(),
            UserId = userId,
            Author = userName,
            Title = createPost.Title,
            Content = createPost.Content
            
        };

        _dbContext.Posts.Add((post));
        await _dbContext.SaveChangesAsync();

        return new CreatePostResponseDto{
            Author = post.Author,
            Title = post.Title, 
            Content = post.Content};
    }
    
    public async Task<List<CurrentUserPostResponseDto>> GetPostsFromCurrentUser(string userId)
    {
        return await _dbContext.Posts.Where(p => p.UserId == userId).Select(p => new CurrentUserPostResponseDto
        {   
            PostId = p.PostId,
            Author = p.Author,
            Title = p.Title,
            Content = p.Content
        }).ToListAsync();
    }
    
    public async Task<UpdatePostResponseDto> UpdatePost(string userId, string postId ,UpdatePostDto updatedPost)
    {   
        var post = await _dbContext.Posts.FindAsync(postId);

        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
        }
        
        if (post.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only edit your own posts");
        }

        if (string.IsNullOrWhiteSpace(updatedPost.Title) || string.IsNullOrWhiteSpace(updatedPost.Content))
        {
            throw new KeyNotFoundException("You must enter either a new title or new content");
        }
        
        post.Title = updatedPost.Title;
        post.Content = updatedPost.Content;

        await _dbContext.SaveChangesAsync();

        return new UpdatePostResponseDto
        {
            Message = "The following post has been updated",
            Post = new PostResponseDto
            {
                Author = post.Author,
                Title = post.Title,
                Content = post.Content
            }
        };
    }
    
    public async Task<string> DeletePost(string userId, string postId)
    {
        var post = await _dbContext.Posts.FindAsync(postId);

        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
        }

        if (post.UserId != userId)
        {
            throw new UnauthorizedAccessException("This is not your post");
        }
        
        _dbContext.Remove(post);
        await _dbContext.SaveChangesAsync();

        return "The post has been removed";

    }

}