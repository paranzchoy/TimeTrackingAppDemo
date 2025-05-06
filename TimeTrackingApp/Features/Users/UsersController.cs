using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingApp.Data;
using TimeTrackingApp.Shared.Dtos.Users;

namespace TimeTrackingApp.Features.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
    {
        var query =  _userManager.Users;
        var users = await query.ToListAsync();
        var dto = users.Select(user => new UserListDto
        {
            UserId = user.Id,
            UserName = user.UserName ??  "no username"
        });

        return Ok(dto);
    }
}
