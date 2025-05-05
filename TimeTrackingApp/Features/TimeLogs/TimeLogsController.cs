using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingApp.Data;

namespace TimeTrackingApp.Features.TimeLogs;

[Route("api/[controller]")]
[ApiController]
public class TimeLogsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public TimeLogsController(ApplicationDbContext context)
    {
        _db = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeLog>>> GetTimeLogs()
    {
        return await _db.TimeLogs.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeLog>> GetTimeLog(int id)
    {
        var timeLog = await _db.TimeLogs.FindAsync(id);

        if (timeLog == null)
        {
            return NotFound();
        }

        return timeLog;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTimeLog(int id, TimeLog timeLog)
    {
        if (id != timeLog.TimeLogId)
        {
            return BadRequest();
        }

        _db.Entry(timeLog).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TimeLogExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TimeLog>> PostTimeLog(TimeLog timeLog)
    {
        _db.TimeLogs.Add(timeLog);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetTimeLog", new { id = timeLog.TimeLogId }, timeLog);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTimeLog(int id)
    {
        var timeLog = await _db.TimeLogs.FindAsync(id);
        if (timeLog == null)
        {
            return NotFound();
        }

        _db.TimeLogs.Remove(timeLog);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool TimeLogExists(int id)
    {
        return _db.TimeLogs.Any(e => e.TimeLogId == id);
    }
}
