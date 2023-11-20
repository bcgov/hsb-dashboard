namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;

public class GroupRoleConfiguration : AuditableConfiguration<GroupRole>
{
    public override void Configure(EntityTypeBuilder<GroupRole> builder)
    {
        builder.ToTable("GroupRole");
        builder.HasKey(m => new { m.GroupId, m.RoleId });
        builder.Property(m => m.GroupId).IsRequired().ValueGeneratedNever();
        builder.Property(m => m.RoleId).IsRequired().ValueGeneratedNever();

        builder.HasOne(m => m.Group).WithMany(m => m.RolesManyToMany).HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany(m => m.GroupsManyToMany).HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
