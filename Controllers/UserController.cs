using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsAPI.DTO.User.Request;
using PostsAPI.DTO.User.Response;
using PostsAPI.Entities;
using PostsAPI.Services;

namespace PostsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    //Get all Users

    [HttpGet("users"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<User>>> GetAllUsers()
        => await _userService.GetAllUsers();

    //Find Users by username

    [HttpGet("users-by-search/{searchTerm}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<List<UserSearchResponseDto>>> GetUsersBySearch(string searchTerm)
        => await _userService.GetUsersBySearch(searchTerm);

    //Find current User

    [HttpGet("current-user"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        var userId = GetUserId();
        return await _userService.GetCurrentUser(userId);
    }
    
    //Register User

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResponseDto>> RegisterUser(RegisterUserDto dto)
        => await _userService.RegisterUser(dto);
    
    //Login User
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseDto>> LoginUser(LoginUserDto dto)
     => await _userService.LoginUser(dto);
    
    
    //Update User

    [HttpPatch("update-user"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<UpdateUserResponseDto>> UpdateUser(UpdateUserDto dto)
    {
        var userId = GetUserId();
        return await _userService.UpdateUser(userId, dto);
    }
    
    //Update User Password

    [HttpPatch("change-password"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<string>> UpdateUserPassword(UpdateUserPasswordDto dto)
    {
        var userId = GetUserId();
        return await _userService.UpdateUserPassword(userId, dto);
    }
    
    
    //Delete User

    [HttpDelete, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<string>> DeleteUser()
    {
        var userId = GetUserId();
        return await _userService.DeleteUser(userId);
    }
}