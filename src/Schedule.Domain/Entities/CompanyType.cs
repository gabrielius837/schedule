namespace Schedule.Domain;

public class CompanyType : BaseEntity<int>
{
    public string Name { get; set; } = default!;
}