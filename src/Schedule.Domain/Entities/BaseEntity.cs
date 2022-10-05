namespace Schedule.Domain;

public abstract class BaseEntity<PK> where PK : struct
{
    public PK Id { get; set; } = default(PK)!;
}