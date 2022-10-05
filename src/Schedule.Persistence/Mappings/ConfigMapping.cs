namespace Schedule.Persistence;

internal class ConfigMapping : IEntityTypeConfiguration<Config>
{
    public void Configure(EntityTypeBuilder<Config> builder)
    {
        builder
            .ToTable("Config");
        builder
            .HasKey(x => new { x.CronMaskId, x.MarketId, x.CompanyTypeId })
            .HasName("PK_Config");
        builder
            .Property(x => x.CronMaskId)
            .IsRequired(true)
            .HasColumnName("CronMaskId");
        builder
            .HasOne(x => x.CronMask)
            .WithMany()
            .HasForeignKey(x => x.CronMaskId)
            //.IsRequired(true)
            .HasConstraintName("FK_Config_CronMask");
        builder
            .Property(x => x.MarketId)
            .IsRequired(true)
            .HasColumnName("MarketId");
        builder
            .HasOne(x => x.Market)
            .WithMany()
            .HasForeignKey(x => x.MarketId)
            //.IsRequired(true)
            .HasConstraintName("FK_Config_Market");
        builder
            .Property(x => x.CompanyTypeId)
            .IsRequired(true)
            .HasColumnName("CompanyTypeId");
        builder
            .HasOne(x => x.CompanyType)
            .WithMany()
            .HasForeignKey(x => x.CompanyTypeId)
            //.IsRequired(true)
            .HasConstraintName("FK_Config_CompanyType");

        builder.HasData
        (
            // Denmark: 5 notifications (sent on days: 1, 5, 10, 15, 20), all company types
            new Config() { CronMaskId = 1, MarketId = 1, CompanyTypeId = 1 },
            new Config() { CronMaskId = 2, MarketId = 1, CompanyTypeId = 1 },
            new Config() { CronMaskId = 4, MarketId = 1, CompanyTypeId = 1 },
            new Config() { CronMaskId = 6, MarketId = 1, CompanyTypeId = 1 },
            new Config() { CronMaskId = 7, MarketId = 1, CompanyTypeId = 1 },
            // Norway: 4 notifications (sent on days: 1, 5, 10, 20), all company types
            new Config() { CronMaskId = 1, MarketId = 2, CompanyTypeId = 1 },
            new Config() { CronMaskId = 2, MarketId = 2, CompanyTypeId = 1 },
            new Config() { CronMaskId = 4, MarketId = 2, CompanyTypeId = 1 },
            new Config() { CronMaskId = 7, MarketId = 2, CompanyTypeId = 1 },
            // Sweden: 4 notifications (sent on days: 1, 7, 14, 28), only for small and medium companies
            new Config() { CronMaskId = 1, MarketId = 3, CompanyTypeId = 2 },
            new Config() { CronMaskId = 3, MarketId = 3, CompanyTypeId = 2 },
            new Config() { CronMaskId = 5, MarketId = 3, CompanyTypeId = 2 },
            new Config() { CronMaskId = 8, MarketId = 3, CompanyTypeId = 2 },

            new Config() { CronMaskId = 1, MarketId = 3, CompanyTypeId = 3 },
            new Config() { CronMaskId = 3, MarketId = 3, CompanyTypeId = 3 },
            new Config() { CronMaskId = 5, MarketId = 3, CompanyTypeId = 3 },
            new Config() { CronMaskId = 8, MarketId = 3, CompanyTypeId = 3 },
            // Finland: 5 notifications (sent on days: 1, 5, 10, 15, 20), only for large companies
            new Config() { CronMaskId = 1, MarketId = 4, CompanyTypeId = 4 },
            new Config() { CronMaskId = 2, MarketId = 4, CompanyTypeId = 4 },
            new Config() { CronMaskId = 4, MarketId = 4, CompanyTypeId = 4 },
            new Config() { CronMaskId = 6, MarketId = 4, CompanyTypeId = 4 },
            new Config() { CronMaskId = 7, MarketId = 4, CompanyTypeId = 4 }
        );
    }
}