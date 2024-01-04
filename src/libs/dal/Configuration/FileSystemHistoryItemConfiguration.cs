namespace HSB.DAL.FileSystem;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using HSB.DAL.Configuration;

public class FileSystemHistoryItemConfiguration : AuditableConfiguration<FileSystemHistoryItem>
{
    public override void Configure(EntityTypeBuilder<FileSystemHistoryItem> builder)
    {
        builder.ToTable("FileSystemHistoryItem");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");
        builder.Property(m => m.RawDataCI).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.ClassName).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Label).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Category).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Subcategory).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.StorageType).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.MediaType).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.VolumeId).IsRequired().HasMaxLength(100).HasDefaultValueSql("''");
        builder.Property(m => m.Capacity).IsRequired();
        builder.Property(m => m.DiskSpace).IsRequired();
        builder.Property(m => m.Size).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.SizeBytes).IsRequired();
        builder.Property(m => m.UsedSizeBytes);
        builder.Property(m => m.AvailableSpace).IsRequired();
        builder.Property(m => m.FreeSpace).IsRequired().HasMaxLength(50).HasDefaultValueSql("''");
        builder.Property(m => m.FreeSpaceBytes).IsRequired();

        builder.HasOne(m => m.FileSystemItem).WithMany(m => m.History).HasForeignKey(m => m.ServiceNowKey).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.CreatedOn }, "IX_FileSystemHistoryItem_CreatedOn");
        builder.HasIndex(m => new { m.ServiceNowKey }, "IX_FileSystemHistoryItem_ServiceNowKey");


        base.Configure(builder);
    }
}
