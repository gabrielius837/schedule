namespace Schedule.Persistence;

internal class CronMaskMapping : IEntityTypeConfiguration<CronMask>
{
    public void Configure(EntityTypeBuilder<CronMask> builder)
    {
        builder
            .ToTable("CronMask");
        builder
            .Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
        builder
            .HasKey(x => x.Id)
            .HasName("PK_CronMask");
        builder
            .Property(x => x.Mask)
            .IsRequired(true)
            .HasColumnName("Mask");
        builder
            .HasIndex(x => x.Mask)
            .HasDatabaseName("UQ_CronMask_Mask")
            .IsUnique(true);
        
        builder.HasData
        (
            new CronMask() { Id = 1, Mask = "0 0 1 * *" }, 
            new CronMask() { Id = 2, Mask = "0 0 5 * *" },
            new CronMask() { Id = 3, Mask = "0 0 7 * *" },
            new CronMask() { Id = 4, Mask = "0 0 10 * *" },
            new CronMask() { Id = 5, Mask = "0 0 14 * *" },
            new CronMask() { Id = 6, Mask = "0 0 15 * *" },
            new CronMask() { Id = 7, Mask = "0 0 20 * *" },
            new CronMask() { Id = 8, Mask = "0 0 28 * *" }
        );
    }
}
