namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;

public class UserTenantConfiguration : AuditableConfiguration<UserTenant>
{
    public override void Configure(EntityTypeBuilder<UserTenant> builder)
    {
        builder.ToTable("UserTenant");
        builder.HasKey(m => new { m.UserId, m.TenantId });
        builder.Property(m => m.UserId).IsRequired().ValueGeneratedNever();
        builder.Property(m => m.TenantId).IsRequired().ValueGeneratedNever();

        builder.HasOne(m => m.User).WithMany(m => m.TenantsManyToMany).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Tenant).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.TenantId).OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
