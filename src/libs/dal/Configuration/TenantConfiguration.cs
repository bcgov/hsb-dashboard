namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;

public class TenantConfiguration : SortableCodeAuditableConfiguration<Tenant, int>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenant");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.ServiceNowKey).IsRequired().HasMaxLength(100);
        builder.Property(m => m.RawData).IsRequired().HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");

        builder.HasMany(m => m.Organizations).WithMany(m => m.Tenants).UsingEntity<TenantOrganization>();

        builder.HasIndex(m => m.Name, "IX_Tenant_Name").IsUnique();
        builder.HasIndex(m => m.Code, "IX_Tenant_Code").IsUnique();
        builder.HasIndex(m => m.ServiceNowKey, "IX_Tenant_ServiceNowKey").IsUnique();

        base.Configure(builder);
    }
}
