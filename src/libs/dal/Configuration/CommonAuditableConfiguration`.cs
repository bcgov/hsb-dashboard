namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;

public class CommonAuditableConfiguration<TEntity, TKey> : AuditableConfiguration<TEntity>
  where TEntity : CommonAuditable<TKey>
  where TKey : notnull
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired();
        builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Description).IsRequired().HasMaxLength(500);
        builder.Property(m => m.IsEnabled).IsRequired();

        base.Configure(builder);
    }
}
