namespace Schedule.Domain;

public class Config
{
    public int CronMaskId { get; set; }
    public CronMask CronMask { get; set; } = default!;
    public int CompanyTypeId { get; set; }
    public CompanyType CompanyType { get; set; } = default!;
    public int MarketId { get; set; }
    public Market Market { get; set; } = default!;
}