using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HSB.Core.Http;
using System.IdentityModel.Tokens.Jwt;

namespace HSB.Ches
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods for ServiceCollection objects.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the AddChesService to the dependency injection service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddChesService(this IServiceCollection services, IConfigurationSection section)
        {
            return services
                .Configure<Configuration.ChesOptions>(section)
                .AddScoped<IChesService, ChesService>()
                .AddScoped<IHttpRequestClient, HttpRequestClient>()
                .AddTransient<JwtSecurityTokenHandler>();
        }

        /// <summary>
        /// Add the AddChesService to the dependency injection service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddChesSingletonService(this IServiceCollection services, IConfigurationSection section)
        {
            return services
                .Configure<Configuration.ChesOptions>(section)
                .AddSingleton<IChesService, ChesService>()
                .AddSingleton<IHttpRequestClient, HttpRequestClient>()
                .AddTransient<JwtSecurityTokenHandler>();
        }
    }
}
