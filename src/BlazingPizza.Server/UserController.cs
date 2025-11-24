using Microsoft.AspNetCore.Mvc;

namespace BlazingPizza.Server;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    [HttpGet]
    public ActionResult<UserInfo> GetCurrentUser()
    {
        return Ok(new UserInfo
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            UserName = User.Identity?.Name,
            UserId = PizzaApiExtensions.GetUserId(HttpContext)
        });
    }
}

public class UserInfo
{
    public bool IsAuthenticated { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
}
