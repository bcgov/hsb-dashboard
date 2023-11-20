namespace HSB.DAL.Server;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using HSB.DAL.Configuration;

public class ServerItemServer : AuditableConfiguration<ServerItem>
{
    public override void Configure(EntityTypeBuilder<ServerItem> builder)
    {
        builder.ToTable("ServerItem");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.ConfigurationItemId).IsRequired();
        builder.Property(m => m.OperatingSystemItemId).IsRequired();
        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Category).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.SubCategory).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.DnsDomain).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.SysClassName).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Platform).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.IPAddress).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.DiskSpace).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");

        builder.HasOne(m => m.OperatingSystemItem).WithMany(m => m.ServerItems).HasForeignKey(m => m.OperatingSystemItemId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.ConfigurationItem).WithMany(m => m.ServerItems).HasForeignKey(m => m.ConfigurationItemId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.ServiceNowKey });

        base.Configure(builder);
    }
}
