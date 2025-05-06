using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TimeTrackingApp.Shared.Clients;
using TimeTrackingApp.Shared.Dtos.Timelogs;

namespace TimeTrackingApp.Client.Features.TimeLogs.EditTimeLog;

public partial class TimeLogEditDialog : ComponentBase, IDialogContentComponent<TimeLogEditDto>
{
    [Parameter]
    public TimeLogEditDto Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Inject]
    public ITimelogsApi TimelogsApi { get; set; } = null!;

    [Inject]
    public IToastService Toast { get; set; } = default!;

    private bool _IsSubmitting = false;
    protected DateOnly SelectedTimeInDateOnly { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    protected TimeOnly SelectedTimeInTimeOnly { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    protected DateOnly SelectedTimeOutDateOnly { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    protected TimeOnly SelectedTimeOutTimeOnly { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

    protected async Task CloseDialog()
    {
        await Dialog.CancelAsync();
    }

    protected async Task OnSubmit()
    {
        //Content.TimeIn = SelectedTimeInDateOnly.ToDateTime(SelectedTimeInTimeOnly);
        Content.TimeOut = SelectedTimeOutDateOnly.ToDateTime(SelectedTimeOutTimeOnly);

        _IsSubmitting = true;

        try
        {
            await TimelogsApi.Update(Content.TimeLogId, Content);
            Toast.ShowSuccess("User Time In created successfully.", timeout: 3200);
            await Dialog.CloseAsync();
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
