namespace Schedule.Persistence;

internal class CompanyTypeMapping : IEntityTypeConfiguration<CompanyType>
{
    public void Configure(EntityTypeBuilder<CompanyType> builder)
    {
        builder
            .ToTable("CompanyType");
        builder
            .Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
        builder
            .HasKey(x => x.Id)
            .HasName("PK_CompanyType");
        builder
            .Property(x => x.Name)
            .IsRequired(true)
            .HasColumnName("Name");
        builder
            .HasIndex(x => x.Name)
            .HasDatabaseName("UQ_CompanyType_Name")
            .IsUnique(true);
        
        builder.HasData
        (
            new CompanyType() { Name = "small" },   //1
            new CompanyType() { Name = "medium" },  //2
            new CompanyType() { Name = "large" }    //3
        );
    }
}
