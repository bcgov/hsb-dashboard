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
        builder.Property(m => m.OrganizationType).IsRequired();
        builder.Property(m => m.ParentId);
        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.HasMany(m => m.Users).WithMany(m => m.Organizations).UsingEntity<UserOrganization>();
        builder.HasOne(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.Name, "IX_organize_name").IsUnique();
        builder.HasIndex(m => m.Code, "IX_organize_code").IsUnique();
        builder.HasIndex(m => m.ServiceNowKey, "IX_organize_serviceNowKey").IsUnique();

        base.Configure(builder);
    }
}
