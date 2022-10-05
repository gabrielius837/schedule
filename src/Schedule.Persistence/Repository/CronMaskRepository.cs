

namespace Schedule.Persistence;

public interface ICronMaskRepository
{
    Task<IReadOnlyDictionary<int, int>> GetIdUnixTimestampMap(DateTime seed, CancellationToken token);
}

public class CronMaskRepository : ICronMaskRepository
{
    private readonly ScheduleContext _context;
    private readonly ILogger<CronMaskRepository> _logger;

    public CronMaskRepository(ScheduleContext context, ILogger<CronMaskRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyDictionary<int, int>> GetIdUnixTimestampMap(DateTime seed, CancellationToken token)
    {
        var dict = new Dictionary<int, int>();
        var masks = await _context.CronMasks.AsNoTracking().ToArrayAsync(token);
        if (masks is null)
        {
            _logger.LogWarning("No cron masks where retrieved from database");
            return dict;
        }
        
        var (lower, upper) = seed.ToMonthDateTimeRange();

        foreach (var mask in masks)       
        {
            var datetime = NextCronOccurence(mask.Mask, seed);
            if (datetime is not null && datetime < upper)
                dict.Add(mask.Id, ((DateTime)datetime).ToUnixTimestamp());
            else
                _logger.LogWarning("Could not resolve for {cron} with {timestmap", mask.Mask, seed.ToString());
        }

        return dict;
    }

    public static DateTime? NextCronOccurence(string mask, DateTime seed)
    {
        var expr = CronExpression.Parse(mask);
        return expr.GetNextOccurrence(seed);
    }
}