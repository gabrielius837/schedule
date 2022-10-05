namespace Schedule.Persistence;

internal class MarketMapping : IEntityTypeConfiguration<Market>
{
    public void Configure(EntityTypeBuilder<Market> builder)
    {
        builder
            .ToTable("Market");
        builder
            .Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
        builder
            .HasKey(x => x.Id)
            .HasName("PK_Market");
        builder
            .Property(x => x.Name)
            .IsRequired(true)
            .HasColumnName("Name");
        builder
            .HasIndex(x => x.Name)
            .HasDatabaseName("UQ_Market_Name")
            .IsUnique(true);
        
        builder.HasData
        (
            new Market() { Id = 1, Name = "Denmark" },
            new Market() { Id = 2, Name = "Norway" },
            new Market() { Id = 3, Name = "Sweden" },
            new Market() { Id = 4, Name = "Finland" }
        );
    }
}
