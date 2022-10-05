namespace Schedule.Persistence;

public interface IScheduleRepository
{
    Task<ScheduleResponse?> GetSchedule(Guid id, DateTime seed, CancellationToken token);
}

public class ScheduleRepository : IScheduleRepository
{
    private readonly ScheduleContext _context;

    public ScheduleRepository(ScheduleContext context)
    {
        _context = context;
    }

    public async Task<ScheduleResponse?> GetSchedule(Guid id, DateTime seed, CancellationToken token)
    {
        var company = await _context.Companies.FindAsync(id, token);
        if (company is null)
            return null;
        
        var (lower, upper) = seed.ToMonthUnixTimestampRange();
        var notifications = await _context.Notifications
            .Where(
                x => 
                    x.CompanyTypeId == company.CompanyTypeId && x.MarketId == company.MarketId &&
                    x.Timestamp > lower && x.Timestamp < upper
            ).ToArrayAsync(token);
        var formattedDates = notifications is not null && notifications.Length > 0
            ? notifications.Select(x => x.Timestamp.ToFormattedDate()).ToArray()
            : Array.Empty<string>();

        var result = new ScheduleResponse(company.Id, formattedDates);
        return result;
    }
}