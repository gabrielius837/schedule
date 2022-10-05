namespace Schedule.Domain;

public class Market : BaseEntity<int>
{
    public string Name { get; set; } = default!;
}