namespace Schedule.Persistence;

public class ScheduleContext : DbContext
{
    public ScheduleContext(DbContextOptions options): base(options)
    {
        
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyType> CompanyTypes => Set<CompanyType>();
    public DbSet<Config> Configs => Set<Config>();
    public DbSet<CronMask> CronMasks => Set<CronMask>();
    public DbSet<Market> Markets => Set<Market>();
    public DbSet<Notification> Notifications => Set<Notification>();
}