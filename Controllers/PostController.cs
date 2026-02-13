using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostsAPI.Data;
using PostsAPI.DTO;
using PostsAPI.Entities;
using PostsAPI.Services;

namespace PostsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : BaseApiController
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    //Fetch All Posts

    [HttpGet("posts"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<List<PostResponseDto>>> GetAllPosts()
       => await _postService.GetAllPosts();
  


    //Create Post

    [HttpPost, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<CreatePostResponseDto>> CreatePost(CreatePostDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _postService.CreatePost(userId, dto);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
    //Find all posts from the current User

    [HttpGet, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<List<Post>>> GetPostsFromCurrentUser()
    {
        try
        {
            var userId = GetUserId();
            var result = await _postService.GetPostsFromCurrentUser(userId);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
    //Update post from the current User

    [HttpPut("{postId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<UpdatePostResponseDto>> UpdatePost(string postId, UpdatePostDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _postService.UpdatePost(userId, postId, dto);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
    //Delete the post from the current list of user's posts

    [HttpDelete("{postId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<string>> DeletePost(string postId)
    {
        try
        {
            var userId = GetUserId();
            var result = await _postService.DeletePost(userId, postId);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    

}