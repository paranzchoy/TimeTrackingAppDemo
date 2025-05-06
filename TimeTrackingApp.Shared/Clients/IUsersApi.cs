using Refit;
using TimeTrackingApp.Shared.Dtos.Users;

namespace TimeTrackingApp.Shared.Clients;

public interface IUsersApi
{
    [Get("/api/users")]
    Task<List<UserListDto>> GetAll();
}
