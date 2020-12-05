using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using RedisCacheService.CacheService;
using RedisCacheService.Configuration;

namespace RedisCacheService.Extensions
{
    /// <summary>
    /// Add Redis cache service dependencies
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Redis dependencies to IServiceCollection
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        /// <param name="configuration">RedisConfiguration instance</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheService(this IServiceCollection services, Func<RedisConfiguration> configuration)
        {
            RedisConfiguration redisConfigurationInstance = configuration.Invoke();

            services.AddSingleton<IRedisCacheService, CacheService.RedisCacheService>();
            services.AddSingleton<RedisConfiguration>(redisConfigurationInstance);

            return services;
        }
    }
}
