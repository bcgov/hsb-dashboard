namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;

public class ConfigurationItemConfiguration : AuditableConfiguration<ConfigurationItem>
{
    public override void Configure(EntityTypeBuilder<ConfigurationItem> builder)
    {
        builder.ToTable("ConfigurationItem");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.TenantId);
        builder.Property(m => m.OrganizationId);
        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Category).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.SubCategory).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Platform).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.DnsDomain).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.ClassName).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.FQDN).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.IPAddress).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");

        builder.HasOne(m => m.Tenant).WithMany(m => m.ConfigurationItems).HasForeignKey(m => m.TenantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Organization).WithMany(m => m.ConfigurationItems).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.ServiceNowKey }, "IX_ConfigurationItem_ServiceNowKey");
        builder.HasIndex(m => new { m.Name }, "IX_ConfigurationItem_Name");

        base.Configure(builder);
    }
}
