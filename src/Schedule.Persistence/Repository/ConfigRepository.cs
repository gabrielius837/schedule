namespace Schedule.Persistence;

public interface IConfigRepository
{
    Task<Config[]> GetConfigs(CancellationToken token);
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

    public async Task<Config[]> GetConfigs(CancellationToken token)
    {
        var list = new List<Config>();
        var types = await _context.CompanyTypes.AsNoTracking().Where(x => x.Id > 1).ToArrayAsync(token);
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

        // Due to lack of data integrity from db
        // I'm trying to ensure entegrity through hash cache
        var cache = new HashSet<int>();
        foreach (var config in configs)
        {
            // if company type is all add the rest
            if (config.CompanyTypeId == 1)
            {
                foreach (var type in types)
                {
                    var hash = CalculateHash(config.CronMaskId, type.Id, config.MarketId);
                    if (!cache.Contains(hash))
                    {
                        list.Add(new Config() { CronMaskId = config.CronMaskId, CompanyTypeId = type.Id, MarketId = config.MarketId });
                        cache.Add(hash);
                    }
                    
                }
            }
            else
            {
                var hash = CalculateHash(config.CronMaskId, config.CompanyTypeId, config.MarketId);
                if (!cache.Contains(hash))
                {
                    list.Add(new Config() { CronMaskId = config.CronMaskId, CompanyTypeId = config.CompanyTypeId, MarketId = config.MarketId });
                    cache.Add(hash);
                }
            }
                
        }

        return list.ToArray();
    }

    public static int CalculateHash(int cronMaskId, int companyTypeId, int marketId) => 397 * cronMaskId ^ companyTypeId ^ marketId;
}