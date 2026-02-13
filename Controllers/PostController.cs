using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostsAPI.Data;
using PostsAPI.DTO.Post.Request;
using PostsAPI.DTO.Post.Response;
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

    [HttpPost("create-post"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<CreatePostResponseDto>> CreatePost(CreatePostDto dto)
    {
        try
        {
            var userId = GetUserId();
            var userName = GetUserName();
            var result = await _postService.CreatePost(userName, userId, dto);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
    //Find all posts from the current User

    [HttpGet("posts-current-user"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<List<PostResponseDto>>> GetPostsFromCurrentUser()
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

    [HttpPut("update-post/{postId}"), Authorize(Roles = "Admin, User")]
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

    [HttpDelete("delete-post/{postId}"), Authorize(Roles = "Admin, User")]
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