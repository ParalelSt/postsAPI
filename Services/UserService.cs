using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostsAPI.DTO.User.Request;
using PostsAPI.DTO.User.Response;
using PostsAPI.Entities;
using PostsAPI.Exceptions;

namespace PostsAPI.Services;

public class UserService: IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    
    public UserService(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    
    public async Task<List<User>> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        return users;
    }

    public async Task<List<UserSearchResponseDto>> GetUsersBySearch(string searchTerm)
    {
        var normalized = _userManager.NormalizeName(searchTerm);
        
        var users = await _userManager.Users.Where(u => 
            u.NormalizedUserName != null && u.NormalizedUserName.Contains(normalized)).ToListAsync();
        
        if (users.Count == 0)
            throw new NotFoundException("User/s not found");

        var result = new List<UserSearchResponseDto>();
        
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserSearchResponseDto 
            {
                UserName = user.UserName ?? string.Empty,
                Roles = roles.ToList()
            });
        }

        return result;
    }
    
    public async Task<User> GetCurrentUser(string userId)
    { 
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
            throw new NotFoundException("User not found");

        return user;
    }
    
    public async Task<RegisterUserResponseDto> RegisterUser(RegisterUserDto user)
    {
        var initials = string.Empty;
        if (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName))
        {
            initials = (user.FirstName[0], user.LastName[0]).ToString().ToUpper();
        }

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new CustomInvalidOperationException("Email is required");

        var existingEmail = await _userManager.FindByEmailAsync(user.Email);
        
        if (existingEmail != null)
        {
            throw new CustomInvalidOperationException("Email already in use");
        }
        
        var createdUser = new User()
        {
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Initials = initials,
            CreatedAt = DateTime.UtcNow
        };

        var create = await _userManager.CreateAsync(createdUser, user.Password);
        if (!create.Succeeded) throw new NotFoundException
            (string.Join(", ", create.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(createdUser, "User");
        
        var roles = await _userManager.GetRolesAsync(createdUser);

        return new RegisterUserResponseDto
        {
            Message = "User created successfully",
            User = new UserResponseDto
            {
                UserName = createdUser.UserName,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt,
                Roles = roles.ToList()
            }
        };
    }
    
    public async Task<LoginUserResponseDto> LoginUser(LoginUserDto loginUser)
    {
        if (string.IsNullOrWhiteSpace(loginUser.Email))
            throw new NotFoundException("Email is required");
        
        var user = await _userManager.FindByEmailAsync(loginUser.Email);
        if (user == null)
        {
            throw new UnauthorizedException("You must be logged in");
        }
        
        if (string.IsNullOrWhiteSpace(loginUser.Password))
            throw new NotFoundException("Password is required");

        var checkUserPassword = await _userManager.CheckPasswordAsync(user, loginUser.Password);

        var roles = await _userManager.GetRolesAsync(user);

        var token = await _tokenService.CreateTokenAsync(user);

        if (loginUser.Email != user.Email)
        {
            throw new UnauthorizedException("Wrong email address");
        }

        if (!checkUserPassword)
        {
            throw new UnauthorizedException("Wrong password");
        }

        return new LoginUserResponseDto
        {
            Message = "Logged in successfully",
            User = new LoginResponseDto
            {
                UserName = user.UserName ?? string.Empty, //IdentityUser.UserName is defined as a "string?"
                Email = user.Email,
                Roles = roles.ToList(),
                Token = token
            }
        };
    }
    
    public async Task<UpdateUserResponseDto> UpdateUser(string userId, UpdateUserDto updatedUser)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found");

        if (!string.IsNullOrWhiteSpace(updatedUser.UserName)) user.UserName = updatedUser.UserName;
        if (!string.IsNullOrWhiteSpace(updatedUser.Email)) user.Email = updatedUser.Email;
        if (!string.IsNullOrWhiteSpace(updatedUser.FirstName)) user.FirstName = updatedUser.FirstName;
        if (!string.IsNullOrWhiteSpace(updatedUser.LastName)) user.LastName = updatedUser.LastName;
        if (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName))
            user.Initials = (user.FirstName[0], user.LastName[0]).ToString().ToUpper();

        if (string.IsNullOrWhiteSpace(updatedUser.UserName) && 
            string.IsNullOrWhiteSpace(updatedUser.FirstName) && 
            string.IsNullOrWhiteSpace(updatedUser.LastName) &&
            string.IsNullOrWhiteSpace(updatedUser.Email))
        {
            throw new NotFoundException("You must provide at least one field to update your user");
        }
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new NotFoundException
            (string.Join(", ", result.Errors.Select(e => e.Description)));

        return new UpdateUserResponseDto
        {
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Initials = user.Initials ?? string.Empty
        };
    }
    
    public async Task<string> UpdateUserPassword(string userId, UpdateUserPasswordDto userPassword)
    {   
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        if (string.IsNullOrWhiteSpace(userPassword.CurrentPassword))
            throw new NotFoundException("Current password is required");

        var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, userPassword.CurrentPassword);
        
        if (string.IsNullOrWhiteSpace(userPassword.NewPassword))
            throw new NotFoundException("New password is required");

        if (!isCurrentPasswordValid)
            throw new CustomInvalidOperationException("The password you have entered does not match the current password");

        if (userPassword.NewPassword == userPassword.CurrentPassword)
            throw new CustomInvalidOperationException("The new password must be different from old password");

        var result = await _userManager.ChangePasswordAsync
            (user, userPassword.CurrentPassword, userPassword.NewPassword);
        if (!result.Succeeded) throw new NotFoundException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return "Password updated successfully";
    }
    
    public async Task<string> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        await _userManager.DeleteAsync(user);

        return "The user has been removed";
    }
}