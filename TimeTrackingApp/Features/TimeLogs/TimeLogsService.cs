using Microsoft.EntityFrameworkCore;
using Refit;
using TimeTrackingApp.Data;
using TimeTrackingApp.Shared.Clients.Timelogs;
using TimeTrackingApp.Shared.Dtos.Timelogs;

namespace TimeTrackingApp.Features.TimeLogs;

public class TimeLogsService : ITimelogsApi
{
    private readonly ApplicationDbContext _db;

    public TimeLogsService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<TimeLogListDto>> GetAll(string userId)
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

        return dtos;
    }

    public async Task<TimeLogCreateDto> Create(TimeLogCreateDto dto)
    {
        var timeLog = new TimeLog
        {
            UserId = dto.UserId,
            TimeIn = dto.TimeIn,
            TimeOut = dto.TimeOut,
        };

        _db.TimeLogs.Add(timeLog);
        await _db.SaveChangesAsync();

        dto.TimeLogId = timeLog.TimeLogId;
        return dto;
    }

    public async Task Update(int timelogId, TimeLogEditDto dto)
    {
        try
        {
            if (timelogId != dto.TimeLogId) throw new Exception("Invalid timelog dto data.");

            var timeLog = await _db.TimeLogs.FindAsync(timelogId) 
                ?? throw new Exception("Time log does not exist.");

            timeLog.TimeOut = dto.TimeOut;

            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TimeLogExists(timelogId))
            {
                throw new InvalidOperationException("TimeLog to update was not found");
            }
            else
            {
                throw;
            }
        }
    }

    private bool TimeLogExists(int id)
    {
        return _db.TimeLogs.Any(g => g.TimeLogId == id);
    }

    public async Task Delete([AliasAs("id")] int timeLogId)
    {
        var timeLog = await _db.TimeLogs.FindAsync(timeLogId);
        if (timeLog == null)
        {
            return;
        }

        _db.TimeLogs.Remove(timeLog);
        await _db.SaveChangesAsync();
    }
}
