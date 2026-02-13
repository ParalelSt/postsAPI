using PostsAPI.DTO.User.Request;
using PostsAPI.DTO.User.Response;
using PostsAPI.Entities;

namespace PostsAPI.Services;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<List<UserSearchResponseDto>> GetUsersBySearch(string searchTerm);
    Task<User> GetCurrentUser(string userId);
    Task<RegisterUserResponseDto> RegisterUser(RegisterUserDto dto);
    Task<LoginUserResponseDto> LoginUser(LoginUserDto dto);
    Task<UpdateUserResponseDto> UpdateUser(string userId, UpdateUserDto dto);
    Task<string> UpdateUserPassword(string userId, UpdateUserPasswordDto dto);
    Task<string> DeleteUser(string userId);
}