namespace HSB.Core.Extensions;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

/// <summary>
/// ModelBuilderExtensions static class, provides extension methods for ModelBuilder objects.
/// </summary>
public static class ModelBuilderExtensions
{
    #region Methods
    /// <summary>
    /// Applies all of the IEntityTypeConfiguration objects in the assembly of the specified type.
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="type"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder, Type type, DbContext context)
    {
        ArgumentNullException.ThrowIfNull(type);
        return modelBuilder.ApplyAllConfigurations(type.Assembly, context);
    }

    /// <summary>
    /// Applies all of the IEntityTypeConfiguration objects in the specified assembly.
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assembly"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder, Assembly assembly, DbContext context)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        // Find all the configuration classes.
        var type = typeof(IEntityTypeConfiguration<>);
        var configurations = assembly.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name.Equals(type.Name)));

        // Fetch the ApplyConfiguration method so that it can be called on each configuration.
        var method = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name.Equals(nameof(ModelBuilder.ApplyConfiguration)) && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == type).First();
        foreach (var config in configurations)
        {
            if (!config.ContainsGenericParameters)
            {
                var includeContext = config.GetConstructors().Any(c => c.GetParameters().Any(p => typeof(DbContext).IsAssignableFrom(p.ParameterType)));
                var entityConfig = includeContext ? Activator.CreateInstance(config, context) : Activator.CreateInstance(config);
                var entityType = config.GetInterfaces().First().GetGenericArguments()[0];
                var applyConfigurationMethod = method.MakeGenericMethod(entityType);
                applyConfigurationMethod.Invoke(modelBuilder, new[] { entityConfig });
            }
        }

        return modelBuilder;
    }

    /// <summary>
    /// Remove pluralizing table name convention.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.BaseType == null)
            {
                entity.SetTableName(entity.DisplayName());
            }
        }
    }
    #endregion
}
