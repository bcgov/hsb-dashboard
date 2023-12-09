namespace HSB.DAL.FileSystem;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using HSB.DAL.Configuration;

public class FileSystemFileSystem : AuditableConfiguration<FileSystemItem>
{
    public override void Configure(EntityTypeBuilder<FileSystemItem> builder)
    {
        builder.ToTable("FileSystemItem");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.ConfigurationItemId).IsRequired();
        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Label).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Category).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.SubCategory).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.StorageType).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.MediaType).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.ClassName).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.VolumeId).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Capacity).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.DiskSpace).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.Size).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.SizeBytes).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.UsedSizeBytes).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.AvailableSpace).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.FreeSpace).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.FreeSpaceBytes).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");

        builder.HasOne(m => m.ConfigurationItem).WithMany(m => m.FileSystemItems).HasForeignKey(m => m.ConfigurationItemId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.ServiceNowKey }, "IX_FileSystemItem_ServiceNowKey");


        base.Configure(builder);
    }
}
