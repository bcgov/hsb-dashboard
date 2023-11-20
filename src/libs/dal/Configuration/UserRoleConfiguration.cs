namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;

public class UserGroupConfiguration : AuditableConfiguration<UserGroup>
{
    public override void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.ToTable("UserGroup");
        builder.HasKey(m => new { m.UserId, m.GroupId });
        builder.Property(m => m.UserId).IsRequired().ValueGeneratedNever();
        builder.Property(m => m.GroupId).IsRequired().ValueGeneratedNever();

        builder.HasOne(m => m.User).WithMany(m => m.GroupsManyToMany).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Group).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
