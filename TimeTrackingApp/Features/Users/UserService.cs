using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeTrackingApp.Data;
using TimeTrackingApp.Shared.Clients;
using TimeTrackingApp.Shared.Dtos.Users;

namespace TimeTrackingApp.Features.Users;

public class UserService : IUsersApi
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserListDto>> GetAll()
    {
        var query = _userManager.Users;
        var users = await query.ToListAsync();
        var dto = users.Select(user => new UserListDto
        {
            UserId = user.Id,
            UserName = user.UserName ?? "no username"
        }).ToList();

        return dto;
    }
}
