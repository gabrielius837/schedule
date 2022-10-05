namespace Schedule.Domain;

public class Company : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;
    public int CompanyTypeId { get; set; }
    public CompanyType CompanyType { get; set; } = default!;
    public int MarketId { get; set; }
    public Market Market { get; set; } = default!;
}