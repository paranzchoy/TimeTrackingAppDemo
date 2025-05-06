using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TimeTrackingApp.Shared.Clients.Timelogs;
using TimeTrackingApp.Shared.Dtos.Users;

namespace TimeTrackingApp.Client.Features.Users.ViewUsers;

public partial class ViewUsersPage : ComponentBase
{
    [Inject]
    public IUsersApi UsersApi { get; set; } = null!;

    [Inject]
    public IToastService ToastService { get; set; } = default!;

    private bool _isLoading = false;
    private List<UserListDto> _users = new();
    private MudBlazor.MudDataGrid<UserListDto> _dataGrid = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _isLoading = true;

        try
        {
            _users = await UsersApi.GetAll();
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Failed to load users: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
        }
    }
}
