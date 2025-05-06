using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TimeTrackingApp.Shared.Clients.Timelogs;
using TimeTrackingApp.Shared.Dtos.Timelogs;

namespace TimeTrackingApp.Client.Features.Users.ViewUserTimelogs;

public partial class UserTimelogsMainView : ComponentBase
{
    [Parameter]
    public string UserId { get; set; } = string.Empty;

    [Parameter]
    public string UserName { get; set; } = string.Empty;

    [Inject]
    private ITimelogsApi TimelogsApi { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    [Inject]
    public IToastService ToastService { get; set; } = default!;


    private bool _IsLoading = false;
    private List<TimeLogListDto> _timelogs = new();


    protected override async Task OnParametersSetAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _IsLoading = true;

        try
        {
            _timelogs = await TimelogsApi.GetAll(UserId);

        }
        catch(Exception ex)
        {
            ToastService.ShowError($"Failed to load timelogs: {ex.Message}");
        }
        finally
        {
            _IsLoading = false;
        }
    }
}
