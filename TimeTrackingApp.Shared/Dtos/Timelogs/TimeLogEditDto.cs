using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackingApp.Shared.Dtos.Timelogs;

public class TimeLogEditDto
{
    [Required]
    public int TimeLogId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public DateTime TimeIn { get; set; }

    public DateTime? TimeOut { get; set; }
}
