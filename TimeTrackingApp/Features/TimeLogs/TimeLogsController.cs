using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingApp.Data;
using TimeTrackingApp.Shared.Dtos.Timelogs;

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

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<TimeLogListDto>>> GetTimeLogs(string userId)
    {
        var timelogs = await _db.TimeLogs
            .Where(e => e.UserId == userId)
            .ToListAsync();

        var dtos = timelogs.Select(e => new TimeLogListDto
        {
            TimeLogId = e.TimeLogId,
            UserId = e.UserId,
            TimeIn = e.TimeIn,
            TimeOut = e.TimeOut,
            TotalHours = e.TotalHours,
        }).ToList();

        return Ok(dtos);
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<TimeLog>> GetTimeLog(int id)
    //{
    //    var timeLog = await _db.TimeLogs.FindAsync(id);

    //    if (timeLog == null)
    //    {
    //        return NotFound();
    //    }

    //    return timeLog;
    //}

    [HttpPost]
    public async Task<ActionResult<TimeLog>> CreateTimeLog(TimeLogCreateDto dto)
    {
        var timeLog = new TimeLog
        {
            UserId = dto.UserId,
            TimeIn = dto.TimeIn,
            TimeOut = dto.TimeOut,
        };

        _db.TimeLogs.Add(timeLog);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetTimeLog", new { id = timeLog.TimeLogId }, timeLog);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTimeLog(int id, TimeLogEditDto dto)
    {
        var timeLog = new TimeLog
        {
            TimeLogId = dto.TimeLogId,
            UserId = dto.UserId,
            TimeIn = dto.TimeIn,
            TimeOut = dto.TimeOut,
        };

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
