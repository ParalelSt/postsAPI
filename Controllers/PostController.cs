using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostsAPI.Data;
using PostsAPI.DTO;
using PostsAPI.Entities;

namespace PostsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    
    private readonly AppDbContext _dbContext;

    public PostController(AppDbContext dbContext) => _dbContext = dbContext;
    
    //Fetch All Posts
    
    [HttpGet("posts"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<List<PostResponseDto>>> GetPosts()
    {
        var posts = await _dbContext.Posts.Select(p => new PostResponseDto
        {
            Title = p.Title,
            Content = p.Content
        }).ToListAsync();
        
        return Ok(posts);
    }
    
    //Create Post

    [HttpPost, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Post>> CreatePost(CreatePostDto createPost)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (createPost.Content == null || createPost.Title == null)
        {
            return BadRequest("You must enter the post details");
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

        return Ok(new { post.Title, post.Content });
    }
    
    //Find all posts from the current User

    [HttpGet, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<List<Post>>> GetPostsFromUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var posts = await _dbContext.Posts.Where(p => p.UserId == userId).ToListAsync();

        return Ok(posts);
    }
    
    //Update post from the current User

    [HttpPut("{postId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Post>> UpdatePost(string postId ,UpdatePostDto updatedPost)
    {   
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var post = await _dbContext.Posts.FindAsync(postId);

        if (post == null)
        {
            return NotFound("Post not found");
        }
        
        if (post.UserId != userId)
        {
            return Unauthorized("You can only edit your own posts");
        }
        
        post.Title = updatedPost.Title;
        post.Content = updatedPost.Content;

        await _dbContext.SaveChangesAsync();

        return Ok(post);
    }
    
    //Delete the post from the current list of user's posts
    
    [HttpDelete("{postId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Post>> DeletePost(string postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = await _dbContext.Posts.FindAsync(postId);

        if (post == null)
        {
            return NotFound("Post not found");
        }

        if (post.UserId != userId)
        {
            return Unauthorized("This is not your post");
        }
        
        _dbContext.Remove(post);
        await _dbContext.SaveChangesAsync();

        return Ok("The post has been removed");

    }

}