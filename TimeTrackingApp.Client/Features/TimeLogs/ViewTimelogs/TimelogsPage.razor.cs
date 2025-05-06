using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Security.Claims;
using TimeTrackingApp.Client.Features.TimeLogs.AddTimeIn;
using TimeTrackingApp.Client.Features.TimeLogs.EditTimeLog;
using TimeTrackingApp.Client.Features.TimeLogs.RemoveTimeLog;
using TimeTrackingApp.Shared.Clients.Timelogs;
using TimeTrackingApp.Shared.Dtos.Timelogs;
using TimeTrackingApp.Shared.Dtos.Users;

namespace TimeTrackingApp.Client.Features.TimeLogs.ViewTimelogs;

public partial class TimelogsPage : ComponentBase
{
    [Inject]
    private ITimelogsApi TimelogsApi { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject]
    public IToastService ToastService { get; set; } = default!;

    private bool _isLoading = false;
    private UserDetailsDto? _User;
    private List<TimeLogListDto> _timelogs = new();


    protected override async Task OnParametersSetAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var userId = await GetUserIdAsync();

        if (string.IsNullOrEmpty(userId))
        {
            ToastService.ShowError("User not found.");
            return;
        }

        _User = new UserDetailsDto
        {
            UserId = userId ?? string.Empty,
        };

        _isLoading = true;

        try
        {
            _timelogs = await TimelogsApi.GetAll(_User!.UserId);
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Failed to load timelogs: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task<string?> GetUserIdAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    protected async Task ShowTimeInAddDialog()
    {
        if (string.IsNullOrEmpty(_User?.UserId))
        {
            ToastService.ShowError("User not found.");
            return;
        }

        DialogParameters parameters = new()
        {
            Modal = true,
            PreventDismissOnOverlayClick = true,
        };

        var data = new TimeLogCreateDto
        {
            UserId = _User!.UserId,
            TimeIn = DateTime.Now,
            TimeOut = null,
        };

        var dialog = await DialogService.ShowDialogAsync<TimeInAddDialog>(data, parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await LoadDataAsync();
            StateHasChanged();
        }
    }

    protected async Task ShowEditDialog(TimeLogListDto dto)
    {
        DialogParameters parameters = new()
        {
            Modal = true,
            PreventDismissOnOverlayClick = true,
        };

        var data = new TimeLogEditDto
        {
            TimeLogId = dto.TimeLogId,
            UserId = dto.UserId,
            TimeIn = dto.TimeIn,
            TimeOut = dto.TimeOut,
        };

        var dialog = await DialogService.ShowDialogAsync<TimeLogEditDialog>(data, parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await LoadDataAsync();
            StateHasChanged();
        }
    }


    protected async Task ShowRemoveDialog(int timelogId)
    {
        var parameters = new DialogParameters
        {
            Title = "Delete time log",
            PrimaryAction = "Delete",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
            Modal = true,
            ShowDismiss = true,
        };

        var dialog = await DialogService.ShowDialogAsync<TimeLogRemoveDialog>(timelogId, parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await DeleteTimelog(timelogId);
            await LoadDataAsync();
            StateHasChanged();
        }
    }

    protected async Task DeleteTimelog(int timeLogId)
    {
        //_IsLoading = true;

        try
        {
            await TimelogsApi.Delete(timeLogId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ToastService.ShowError("Internal service error. Try again later.");
        }
        finally
        {
        }
    }
}