using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PostsAPI.DTO;
using PostsAPI.Entities;

namespace PostsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public UserController(UserManager<User> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }
    
    private string CreateToken(User user)
    {
        var roles = _userManager.GetRolesAsync(user).Result;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    public ActionResult<List<User>> GetAllUsers()
    {
        var users = _userManager.Users.ToList();
        return Ok(users);
    }

    [HttpGet("{userName}")]
    public async Task<IActionResult> GetUser(string userName)
    {
        var normalized = _userManager.NormalizeName(userName);
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => 
            u.NormalizedUserName != null && u.NormalizedUserName.Contains(normalized));
        
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.UserName,
            Roles = roles
        });
    }
    
    //Register User

    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(RegisterUserDto user)
    {
        var initials = (user.FirstName[0], user.LastName[0]).ToString().ToUpper();
        
        var createdUser = new User()
        {
            UserName = user.Email,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Initials = initials,
            CreatedAt = DateTime.UtcNow
        };

        var create = await _userManager.CreateAsync(createdUser, user.Password!);
        if (!create.Succeeded) return BadRequest(create.Errors);

        await _userManager.AddToRoleAsync(createdUser, "User");
        
        var roles = await _userManager.GetRolesAsync(createdUser);

        return Ok(new
        {
            createdUser, Roles = roles
        });
    }
    
    //Login User
    
    [HttpPost("login")]
    public async Task<ActionResult<User>> LogInUser(LoginUserDto loginUser)
    {
        var user = await _userManager.FindByNameAsync(loginUser.UserName);
        if (user == null)
        {
            return Unauthorized();
        }

        var checkUserPassword = await _userManager.CheckPasswordAsync(user, loginUser.Password);

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("User"))
        {
            return Forbid();
        }

        var token = CreateToken(user);

        return Ok(new { user, token });
    }
}
