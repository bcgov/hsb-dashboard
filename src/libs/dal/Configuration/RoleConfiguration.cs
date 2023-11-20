namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;

public class RoleConfiguration : SortableAuditableConfiguration<Role, int>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.Key).IsRequired().HasDefaultValueSql("uuid_generate_v1()");

        builder.HasIndex(m => new { m.IsEnabled }, "IX_role");
        builder.HasIndex(m => new { m.Name }, "IX_role_name").IsUnique();
        builder.HasIndex(m => m.Key, "IX_role_key").IsUnique();

        base.Configure(builder);
    }
}
