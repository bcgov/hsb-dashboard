namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;

public class UserConfiguration : AuditableConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(m => m.Username).IsRequired().HasMaxLength(50);
        builder.Property(m => m.Email).IsRequired().HasMaxLength(100);
        builder.Property(m => m.EmailVerified).IsRequired();
        builder.Property(m => m.EmailVerifiedOn);
        builder.Property(m => m.Key).IsRequired().HasDefaultValueSql("uuid_generate_v1()");
        builder.Property(m => m.DisplayName).IsRequired().HasMaxLength(50);
        builder.Property(m => m.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(m => m.MiddleName).IsRequired().HasMaxLength(50);
        builder.Property(m => m.LastName).IsRequired().HasMaxLength(50);
        builder.Property(m => m.Phone).IsRequired().HasMaxLength(15);
        builder.Property(m => m.IsEnabled).IsRequired();
        builder.Property(m => m.FailedLogins).IsRequired().HasDefaultValueSql("0");
        builder.Property(m => m.LastLoginOn);

        builder.HasMany(m => m.Groups).WithMany(m => m.Users).UsingEntity<UserGroup>();
        builder.HasMany(m => m.Tenants).WithMany(m => m.Users).UsingEntity<UserTenant>();

        builder.HasIndex(m => m.Username, "IX_user_username").IsUnique();
        builder.HasIndex(m => m.DisplayName, "IX_user_display_name").IsUnique();
        builder.HasIndex(m => m.Key, "IX_user_key").IsUnique();
        builder.HasIndex(m => new { m.Email, m.Phone, m.LastName, m.FirstName });

        base.Configure(builder);
    }
}
