namespace TimeTrackingApp.Shared.Dtos.Timelogs;

public class TimeLogListDto
{
    public int TimeLogId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public DateTime TimeIn { get; set; }

    public DateTime? TimeOut { get; set; }

    public double? TotalHours { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
