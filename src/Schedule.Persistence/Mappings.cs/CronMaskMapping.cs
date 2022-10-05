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
            new CronMask() { Mask = "0 0 1 * *" },  //1
            new CronMask() { Mask = "0 0 5 * *" },  //2
            new CronMask() { Mask = "0 0 7 * *" },  //3
            new CronMask() { Mask = "0 0 10 * *" }, //4
            new CronMask() { Mask = "0 0 14 * *" }, //5
            new CronMask() { Mask = "0 0 15 * *" }, //6
            new CronMask() { Mask = "0 0 20 * *" }, //7
            new CronMask() { Mask = "0 0 28 * *" }  //8
        );
    }
}
