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
            new Market() { Name = "Denmark" },  //1
            new Market() { Name = "Norway" },   //2
            new Market() { Name = "Sweden" },   //3
            new Market() { Name = "Finland" }   //4
        );
    }
}
