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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScheduleContext).Assembly);
    }
}

public class ScheduleContextFactory : IDesignTimeDbContextFactory<ScheduleContext>
{
    public ScheduleContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ScheduleContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=Schedule;Trusted_Connection=True;");

        return new ScheduleContext(optionsBuilder.Options);
    }
}