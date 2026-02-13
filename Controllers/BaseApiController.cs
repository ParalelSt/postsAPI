using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PostsAPI.Entities;

namespace PostsAPI.Controllers;

public class BaseApiController: ControllerBase
{
    protected string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    protected string GetUserName()
    {
        return User.FindFirstValue(ClaimTypes.Name);
    }
}