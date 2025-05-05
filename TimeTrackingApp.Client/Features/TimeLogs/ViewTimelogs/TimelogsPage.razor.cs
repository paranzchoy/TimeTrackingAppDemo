using Microsoft.AspNetCore.Components;
using TimeTrackingApp.Shared.Clients.Timelogs;
using TimeTrackingApp.Shared.Dtos.Timelogs;

namespace TimeTrackingApp.Client.Features.TimeLogs.ViewTimelogs;

public partial class TimelogsPage : ComponentBase
{
    [Inject]
    private ITimelogsApi TimelogsApi { get; set; } = null!;
    private List<TimeLogListDto> _timelogs = new();

    protected override async Task OnInitializedAsync()
    {
        _timelogs = await TimelogsApi.GetAll();
    }
}
