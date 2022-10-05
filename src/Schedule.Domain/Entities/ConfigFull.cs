namespace Schedule.Domain;

public class ConfigFull
{
    public ConfigFull(int cronMaskId, int companyTypeId, int marketId)
    {
        cronMaskId = CronMaskId;
        companyTypeId = CompanyTypeId;
        marketId = MarketId;
    }

    public int CronMaskId { get; }
    public int CompanyTypeId { get; }
    public int MarketId { get; }
}