using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PostsAPI.Data;
using PostsAPI.DTO;
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
            Title = p.Title,
            Content = p.Content
        }).ToListAsync();
        
        return posts;
    }
    
    public async Task<CreatePostResponseDto> CreatePost(string userId, CreatePostDto createPost)
    {
        if (createPost.Content == null || createPost.Title == null)
        {
            throw new KeyNotFoundException("You must enter the post details");
        }

        var post = new Post
        {   
            PostId = Guid.NewGuid().ToString(),
            UserId = userId,
            Title = createPost.Title,
            Content = createPost.Content
            
        };

        _dbContext.Posts.Add((post));
        await _dbContext.SaveChangesAsync();

        return new CreatePostResponseDto{ Title = post.Title, Content = post.Content};
    }
    
    public async Task<List<Post>> GetPostsFromCurrentUser(string userId)
    {
        var posts = await _dbContext.Posts.Where(p => p.UserId == userId).ToListAsync();

        return posts;
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
        
        post.Title = updatedPost.Title;
        post.Content = updatedPost.Content;

        await _dbContext.SaveChangesAsync();

        return new UpdatePostResponseDto
        {
            Message = "The following post has been updated",
            Post = post
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