using PostsAPI.Entities;

namespace PostsAPI.Services;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
}