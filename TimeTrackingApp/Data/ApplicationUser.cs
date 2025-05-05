using Microsoft.AspNetCore.Identity;

namespace TimeTrackingApp.Data;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
}
