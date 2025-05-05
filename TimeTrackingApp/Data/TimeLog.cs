using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrackingApp.Data;

[Table("TimeLogs", Schema ="DTR")]
public class TimeLog
{
    [Key]
    public int TimeLogId { get; set; }


    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    [Required]
    public DateTime TimeIn { get; set; }

    public DateTime? TimeOut { get; set; } 

    public double? TotalHours => TimeOut.HasValue
        ? (TimeOut.Value - TimeIn).TotalHours
        : null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
