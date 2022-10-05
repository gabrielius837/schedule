namespace Schedule.Persistence;

internal class CompanyMapping : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .ToTable("Company");
        builder
            .Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired(true);
        builder
            .HasKey(x => x.Id)
            .HasName("PK_Company");
        builder
            .Property(x => x.Name)
            .IsRequired(true)
            .HasColumnName("Name");
        builder
            .HasIndex(x => x.Name)
            .HasDatabaseName("UQ_Company_Name")
            .IsUnique(true);
        builder
            .Property(x => x.Number)
            .IsRequired(true)
            .HasColumnName("Name");
        builder
            .HasCheckConstraint("CK_Company_Number", "LEN(Number) = 10 AND Number LIKE '[0-9]'")
            .HasIndex(x => x.Number)
            .HasDatabaseName("UQ_Company_Number")
            .IsUnique(true);
        builder
            .Property(x => x.CompanyTypeId)
            .IsRequired(true)
            .HasColumnName("CompanyTypeId");
        builder
            .HasOne(x => x.CompanyType)
            .WithMany()
            .HasForeignKey(x => x.CompanyTypeId)
            .IsRequired(true)
            .HasConstraintName("FK_Company_CompanyType");
        builder
            .Property(x => x.MarketId)
            .IsRequired(true)
            .HasColumnName("MarketId");
        builder
            .HasOne(x => x.Market)
            .WithMany()
            .HasForeignKey(x => x.MarketId)
            .IsRequired(true)
            .HasConstraintName("FK_Company_Market");

        builder.HasData
        (
            new Company()
            {
                Id = new Guid("aad7a630-af1c-4952-9cb4-44b8b847853b"),
                Name = "scheduled company",
                Number = "0123456789",
                // small
                CompanyTypeId = 1,
                // Denmark
                MarketId = 1
            },
            new Company()
            {
                Id = new Guid("54142eda-2b7c-43bb-83f4-5dc79dba5988"),
                Name = "unscheduled company",
                Number = "1231231234",
                // large
                CompanyTypeId = 3,
                // Sweden
                MarketId = 3
            },
            new Company()
            {
                Id = new Guid("ffe5ffdd-9a9e-4be4-88ac-b90614b04ce8"),
                Name = "*all* company",
                Number = "4564564567",
                // small
                CompanyTypeId = 2,
                // Norway
                MarketId = 2
            }
        );
    }
}