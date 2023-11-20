namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;

public class GroupConfiguration : SortableAuditableConfiguration<Group, int>
{
    public override void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Group");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.Key).IsRequired().HasDefaultValueSql("uuid_generate_v1()");

        builder.HasMany(m => m.Roles).WithMany(m => m.Groups).UsingEntity<GroupRole>();

        builder.HasIndex(m => new { m.IsEnabled }, "IX_group");
        builder.HasIndex(m => new { m.Name }, "IX_group_name").IsUnique();
        builder.HasIndex(m => m.Key, "IX_group_key").IsUnique();

        base.Configure(builder);
    }
}
