using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using RedisCacheService.Configuration;
using StackExchange.Redis;

namespace RedisCacheService.Server
{
    public class RedisServer
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisServer(RedisConfiguration redisConfiguration)
        {
            string connectionString = CreateRedisConfigurationString(redisConfiguration);

            _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

            if (!_connectionMultiplexer.IsConnected)
            {
                throw new Exception($"ConnectionMultiplexer didn't connect! Please check configuration settings in {redisConfiguration.GetType().Name}");
            }
        }

        public IDatabase GetDatabase(int databaseId)
        {
            if (databaseId < 0) throw new ArgumentException("Database id must greater than or equal to 0.");
            if (databaseId > 15) throw new ArgumentException("Database id must less than or equal to 15.");

            return _connectionMultiplexer.GetDatabase(databaseId);
        }

        private string CreateRedisConfigurationString(RedisConfiguration redisConfiguration)
        {
            var redisHostConfigurationSection = redisConfiguration.Host;
            var redisPortConfigurationSection = redisConfiguration.Port;

            if (redisHostConfigurationSection == default || redisPortConfigurationSection == default)
            {
                throw new Exception($"Redis configuration informations can not be null. Please check {redisConfiguration.GetType().Name}");
            }

            return $"{redisHostConfigurationSection}:{redisPortConfigurationSection}";
        }
    }
}
