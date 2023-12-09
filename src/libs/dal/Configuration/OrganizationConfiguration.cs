namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;

public class OrganizationConfiguration : SortableCodeAuditableConfiguration<Organization, int>
{
    public override void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organization");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.ParentId);
        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.HasOne(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.Name, "IX_Organization_Name").IsUnique();
        builder.HasIndex(m => m.Code, "IX_Organization_Code").IsUnique();
        builder.HasIndex(m => m.ServiceNowKey, "IX_Organization_ServiceNowKey").IsUnique();

        base.Configure(builder);
    }
}
