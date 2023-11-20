namespace HSB.DAL.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HSB.Entities;

public class SortableCodeAuditableConfiguration<TEntity, TKey> : SortableAuditableConfiguration<TEntity, TKey>
  where TEntity : SortableCodeAuditable<TKey>
  where TKey : notnull
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(m => m.Code).IsRequired().HasMaxLength(10);

        base.Configure(builder);
    }
}
