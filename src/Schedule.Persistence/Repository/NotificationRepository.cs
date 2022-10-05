namespace Schedule.Persistence;

public interface INotificationRepository
{
    Task<int> WriteNotifications(DateTime seed, CancellationToken token);
}

public class NotificationRepository : INotificationRepository
{
    private readonly ScheduleContext _context;
    private readonly ILogger<NotificationRepository> _logger;
    private readonly IConfigRepository _configRepository;
    private readonly ICronMaskRepository _cronMaskRepository;

    public NotificationRepository
    (
        ScheduleContext context,
        ILogger<NotificationRepository> logger,
        IConfigRepository configRepository,
        ICronMaskRepository cronMaskRepository
    )
    {
        _context = context;
        _logger = logger;
        _configRepository = configRepository;
        _cronMaskRepository = cronMaskRepository;
    }

    public async Task<int> WriteNotifications(DateTime seed, CancellationToken token)
    {
        var (lower, upper) = seed.ToMonthUnixTimestampRange();
        var existing = await _context.Notifications.AsNoTracking()
            .Where(x => x.Timestamp >= lower && x.Timestamp <= upper)
            .ToArrayAsync();

        var cache = existing.Select(x => CalculateHash(x)).ToHashSet();
        var configs = await _configRepository.GetConfigs(token);
        var cronMap = await _cronMaskRepository.GetIdUnixTimestampMap(seed, token);

        var list = new List<Notification>();
        foreach (var config in configs)
        {
            if (cronMap.ContainsKey(config.CronMaskId))
                list.Add(new Notification()
                { 
                    CompanyTypeId = config.CompanyTypeId,
                    MarketId = config.MarketId,
                    Timestamp = cronMap[config.CronMaskId]
                });
        }

        var newNotifications = list.Where(x => !cache.Contains(CalculateHash(x))).ToArray();

        if (newNotifications is null || newNotifications.Length == 0)
            return 0;
        
        await _context.Notifications.AddRangeAsync(newNotifications);
        var count = await _context.SaveChangesAsync();
        if (count > 0)
            _logger.LogInformation("{count} new notification entries have been written to database", count);
        return count;
    }

    private static int CalculateHash(Notification notification)
        => 397 * notification.CompanyTypeId ^ notification.MarketId ^ notification.Timestamp;
}