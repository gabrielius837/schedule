namespace Schedule.WebApi.Controllers;

[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly ILogger<ScheduleController> _logger;
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleController(ILogger<ScheduleController> logger, IScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
    }

    [HttpGet("/schedule/{companyId}")]
    public async Task<IActionResult> GetSchedule(Guid companyId, CancellationToken token = default)
    {
        var schedule = await _scheduleRepository.GetSchedule(companyId, DateTime.UtcNow, token);
        _logger.LogInformation("Schedule has been requested for: {id}", companyId);
        if (schedule is not null)
        {
            return Ok(schedule);
        }
        else
        {
            var response = new ErrorResponse(HttpContext.TraceIdentifier, 404, $"Company was not found");
            return NotFound(response);
        }
    }
}
