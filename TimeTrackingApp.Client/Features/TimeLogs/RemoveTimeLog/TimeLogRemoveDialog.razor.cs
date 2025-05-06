using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TimeTrackingApp.Shared.Clients;

namespace TimeTrackingApp.Client.Features.TimeLogs.RemoveTimeLog;

public partial class TimeLogRemoveDialog : ComponentBase, IDialogContentComponent<int>
{
    [Parameter]
    public int Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Inject]
    public ITimelogsApi TimelogsApi { get; set; } = null!;

    [Inject]
    public IToastService Toast { get; set; } = default!;

    private bool _IsSubmitting = false;

    protected async Task CloseDialog()
    {
        await Dialog.CancelAsync();
    }

    protected async Task OnSubmit()
    {
        _IsSubmitting = true;

        try
        {
            await TimelogsApi.Delete(Content);
            Toast.ShowSuccess("Timelog deleted successfully.", timeout: 3200);
            await Dialog.CloseAsync();
        }
        finally
        {
            _IsSubmitting = false;
        }

    }
}
