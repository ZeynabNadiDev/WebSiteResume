using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Infra.Data.Services.Caching;

namespace Resume.Infra.Data.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
      
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString!);
            });

     
            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
