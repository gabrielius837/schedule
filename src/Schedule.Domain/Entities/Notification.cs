namespace Schedule.Domain;

public class Notification
{
    public int CompanyTypeId { get; set; }
    public CompanyType CompanyType { get; set; } = default!;
    public int MarketId { get; set; }
    public Market Market { get; set; } = default!;
    public int Timestamp { get; set; }
}