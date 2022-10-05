namespace Schedule.Domain;

public class CronMask : BaseEntity<int>
{
    public string Mask { get; set; } = default!;
}