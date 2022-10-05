namespace Schedule.Persistence;

public interface IConfigRepository
{
    Task<ConfigFull[]> GetConfigFull(CancellationToken token);
}

public class ConfigRepository : IConfigRepository
{

    private readonly ScheduleContext _context;
    private readonly ILogger<ConfigRepository> _logger;

    public ConfigRepository(ScheduleContext context, ILogger<ConfigRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ConfigFull[]> GetConfigFull(CancellationToken token)
    {
        var list = new List<ConfigFull>();
        var types = await _context.CompanyTypes.AsNoTracking().ToArrayAsync(token);
        if (types is null || types.Length == 0)
        {
            _logger.LogWarning("No company types were retrieved");
            return list.ToArray();
        }

        var configs = await _context.Configs.AsNoTracking().ToArrayAsync();
        if (configs is null || configs.Length == 0)
        {
            _logger.LogWarning("No company types were retrieved");
            return list.ToArray();
        }

        foreach (var config in configs)
        {
            if (config.CompanyTypeId is null)
            {
                foreach (var type in types)
                    list.Add(new ConfigFull(config.CronMaskId, type.Id, config.MarketId));
            }
            else
                list.Add(new ConfigFull(config.CronMaskId, (int)config.CompanyTypeId, config.MarketId));
        }

        return list.ToArray();
    }
}