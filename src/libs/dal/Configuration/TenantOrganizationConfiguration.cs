namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;

public class TenantOrganizationConfiguration : AuditableConfiguration<TenantOrganization>
{
    public override void Configure(EntityTypeBuilder<TenantOrganization> builder)
    {
        builder.ToTable("TenantOrganization");
        builder.HasKey(m => new { m.TenantId, m.OrganizationId });
        builder.Property(m => m.TenantId).IsRequired().ValueGeneratedNever();
        builder.Property(m => m.OrganizationId).IsRequired().ValueGeneratedNever();

        builder.HasOne(m => m.Tenant).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.TenantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Organization).WithMany(m => m.TenantsManyToMany).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
