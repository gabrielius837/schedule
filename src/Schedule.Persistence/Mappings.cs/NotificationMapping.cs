namespace Schedule.Persistence;

internal class NotificationMapping : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .ToTable("Notification");
        builder
            .HasKey(x => new { x.MarketId, x.CompanyTypeId, x.Timestamp })
            .HasName("PK_Notification");
        builder
            .Property(x => x.MarketId)
            .IsRequired(true)
            .HasColumnName("MarketId");
        builder
            .HasOne(x => x.Market)
            .WithMany()
            .HasForeignKey(x => x.MarketId)
            .IsRequired(true)
            .HasConstraintName("FK_Notification_Market");
        builder
            .Property(x => x.CompanyTypeId)
            .IsRequired(true)
            .HasColumnName("CompanyTypeId");
        builder
            .HasOne(x => x.CompanyType)
            .WithMany()
            .HasForeignKey(x => x.CompanyTypeId)
            .IsRequired(true)
            .HasConstraintName("FK_Notification_CompanyType");
        builder
            .Property(x => x.Timestamp)
            .IsRequired(true)
            .HasColumnName("Timestamp");
    }
}