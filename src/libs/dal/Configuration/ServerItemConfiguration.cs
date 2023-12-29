namespace HSB.DAL.Server;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using HSB.DAL.Configuration;

public class ServerItemConfiguration : AuditableConfiguration<ServerItem>
{
    public override void Configure(EntityTypeBuilder<ServerItem> builder)
    {
        builder.ToTable("ServerItem");
        builder.HasKey(m => m.ServiceNowKey);
        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100).ValueGeneratedNever();
        builder.Property(m => m.TenantId);
        builder.Property(m => m.OrganizationId);
        builder.Property(m => m.OperatingSystemItemId);

        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");
        builder.Property(m => m.RawDataCI).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

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

        builder.HasOne(m => m.Tenant).WithMany(m => m.ServerItems).HasForeignKey(m => m.TenantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Organization).WithMany(m => m.ServerItems).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.OperatingSystemItem).WithMany(m => m.ServerItems).HasForeignKey(m => m.OperatingSystemItemId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.Name }, "IX_ServerItem_Name");

        base.Configure(builder);
    }
}
