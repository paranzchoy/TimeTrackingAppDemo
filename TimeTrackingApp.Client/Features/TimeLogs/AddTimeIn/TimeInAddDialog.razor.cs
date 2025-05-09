﻿using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TimeTrackingApp.Shared.Clients;
using TimeTrackingApp.Shared.Dtos.Timelogs;

namespace TimeTrackingApp.Client.Features.TimeLogs.AddTimeIn;

public partial class TimeInAddDialog : ComponentBase, IDialogContentComponent<TimeLogCreateDto>
{
    [Parameter]
    public TimeLogCreateDto Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Inject]
    public ITimelogsApi TimelogsApi { get; set; } = null!;

    [Inject]
    public IToastService Toast { get; set; } = default!;

    private bool _IsSubmitting = false;
    protected DateOnly SelectedTimeInDateOnly { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    protected TimeOnly SelectedTimeInTimeOnly { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

    protected async Task CloseDialog()
    {
        await Dialog.CancelAsync();
    }

    protected async Task OnSubmit()
    {
        if (_IsSubmitting) return;

        Content.TimeIn = SelectedTimeInDateOnly.ToDateTime(SelectedTimeInTimeOnly);

        _IsSubmitting = true;

        try
        {
            var result = await TimelogsApi.Create(Content);
            Toast.ShowSuccess("User Time In created successfully.", timeout: 3200);
            await Dialog.CloseAsync(result);
        }
        catch (Exception ex)
        {
            Toast.ShowError($"Failed to create User Time In: {ex.Message}");
        }
        finally
        {
            _IsSubmitting = false;
        }
    }
}
