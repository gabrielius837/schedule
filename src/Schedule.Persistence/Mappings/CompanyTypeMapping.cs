namespace Schedule.Persistence;

internal class CompanyTypeMapping : IEntityTypeConfiguration<CompanyType>
{
    public void Configure(EntityTypeBuilder<CompanyType> builder)
    {
        builder
            .ToTable("CompanyType");
        builder
            .Property(x => x.Id)
            .HasColumnName("Id");
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
            new CompanyType() { Id = 1, Name = "all" },     //1
            new CompanyType() { Id = 2, Name = "small" },   //2
            new CompanyType() { Id = 3, Name = "medium" },  //3
            new CompanyType() { Id = 4, Name = "large" }    //4
        );
    }
}
