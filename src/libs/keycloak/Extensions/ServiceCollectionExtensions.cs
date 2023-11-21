using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HSB.Keycloak.Configuration;

namespace HSB.Keycloak
{
  /// <summary>
  /// ServiceCollectionExtensions static class, provides extension methods for ServiceCollectionExtensions objects.
  /// </summary>
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Add the PimsKeycloakService to the dependency injection service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddKeycloakService(this IServiceCollection services, IConfiguration config)
    {
      return services
          .Configure<KeycloakOptions>(config)
          .AddScoped<IKeycloakService, KeycloakService>();
    }
  }
}
