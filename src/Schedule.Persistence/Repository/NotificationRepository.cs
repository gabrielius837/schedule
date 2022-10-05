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
        var existingNotifications = await _context.Notifications.AsNoTracking().Where(x => x.Timestamp > lower && x.Timestamp < upper).ToArrayAsync();
        var configs = await _configRepository.GetConfigFull(token);
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
        var existingSet = existingNotifications is not null
            ? existingNotifications.Select(x => CalculateHash(x)).ToHashSet()
            : new HashSet<int>();

        var newNotifications = list.Where(x => !existingSet.Contains(CalculateHash(x))).ToArray();

        if (newNotifications is null || newNotifications.Length == 0)
            return 0;
        
        await _context.Notifications.AddRangeAsync(newNotifications);
        return await _context.SaveChangesAsync();
    }

    private static int CalculateHash(Notification notification)
        => notification.CompanyTypeId ^ notification.MarketId ^ notification.Timestamp;
}