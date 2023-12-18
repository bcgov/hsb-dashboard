namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;

public class DataSyncConfiguration : CommonAuditableConfiguration<DataSync, int>
{
    public override void Configure(EntityTypeBuilder<DataSync> builder)
    {
        builder.ToTable("DataSync");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.Offset).IsRequired();
        builder.Property(m => m.Query).IsRequired().HasColumnType("text");

        builder.HasIndex(m => new { m.IsEnabled }, "IX_DataSync");
        builder.HasIndex(m => new { m.Name }, "IX_DataSync_Name").IsUnique();

        base.Configure(builder);
    }
}
