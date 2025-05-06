using Refit;
using TimeTrackingApp.Shared.Dtos.Timelogs;

namespace TimeTrackingApp.Shared.Clients;

public interface ITimelogsApi
{
    [Get("/api/timelogs/{userid}")]
    Task<List<TimeLogListDto>> GetAll([AliasAs("userid")] string userId);

    [Post("/api/timelogs")]
    Task<TimeLogCreateDto> Create([Body] TimeLogCreateDto dto);

    [Patch("/api/timelogs/{id}")]
    Task Update([AliasAs("id")] int TimelogId, TimeLogEditDto dto);

    [Delete("/api/timelogs/{id}")]
    Task Delete([AliasAs("id")] int TimelogId);
}
