namespace HSB.DAL.OperatingSystem;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using HSB.DAL.Configuration;

public class OperatingSystemItemConfiguration : AuditableConfiguration<OperatingSystemItem>
{
    public override void Configure(EntityTypeBuilder<OperatingSystemItem> builder)
    {
        builder.ToTable("OperatingSystemItem");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(200);

        builder.HasIndex(m => new { m.ServiceNowKey }, "IX_OperatingSystemItem_ServiceNowKey").IsUnique();
        builder.HasIndex(m => new { m.Name }, "IX_OperatingSystemItem_Name");

        base.Configure(builder);
    }
}
