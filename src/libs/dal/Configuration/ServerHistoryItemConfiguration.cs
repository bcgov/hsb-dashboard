namespace HSB.DAL.Server;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using HSB.DAL.Configuration;

public class ServerHistoryItemConfiguration : AuditableConfiguration<ServerHistoryItem>
{
    public override void Configure(EntityTypeBuilder<ServerHistoryItem> builder)
    {
        builder.ToTable("ServerHistoryItem");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.TenantId);
        builder.Property(m => m.OrganizationId);
        builder.Property(m => m.OperatingSystemItemId);
        builder.Property(m => m.HistoryKey);
        builder.Property(m => m.InstallStatus).IsRequired();

        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");
        builder.Property(m => m.RawDataCI).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.ClassName).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Category).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Subcategory).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.DnsDomain).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Platform).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.IPAddress).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.FQDN).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.DiskSpace);

        builder.Property(m => m.Capacity);
        builder.Property(m => m.AvailableSpace);

        builder.HasOne(m => m.ServerItem).WithMany(m => m.History).HasForeignKey(m => m.ServiceNowKey).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Tenant).WithMany().HasForeignKey(m => m.TenantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Organization).WithMany().HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.OperatingSystemItem).WithMany().HasForeignKey(m => m.OperatingSystemItemId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.CreatedOn }, "IX_ServerHistoryItem_CreatedOn");
        builder.HasIndex(m => new { m.InstallStatus, m.ServiceNowKey }, "IX_ServerHistoryItem_InstallStatus_ServiceNowKey");
        builder.HasIndex(m => new { m.HistoryKey }, "IX_ServerHistoryItem_HistoryKey");

        base.Configure(builder);
    }
}
