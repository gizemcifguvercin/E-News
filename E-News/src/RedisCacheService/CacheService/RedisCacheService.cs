using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RedisCacheService.Server;

namespace RedisCacheService.CacheService
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly RedisServer _redisServer;

        public RedisCacheService(RedisServer redisServer)
        {
            _redisServer = redisServer;
        }

        public void Add(string key, object data, int expiry = 60, int databaseId = 0)
        {
            string parsedJsonData = JsonConvert.SerializeObject(data);

            _redisServer
                .GetDatabase(databaseId)
                .StringSet(key, parsedJsonData, TimeSpan.FromMinutes(expiry));
        }

        public T Get<T>(string key, int databaseId = 0)
        {
            string parsedJsonData = _redisServer.GetDatabase(databaseId).StringGet(key);
            return JsonConvert.DeserializeObject<T>(parsedJsonData);
        }

        public void Remove(string key, int databaseId = 0)
        {
            _redisServer.GetDatabase(databaseId).KeyDelete(key);
        }

        public void AddHash(string hashKey, string key, object data, int databaseId = 0)
        {
            string parsedJsonData = JsonConvert.SerializeObject(data);

            _redisServer
                .GetDatabase(databaseId)
                .HashSet(hashKey, key, parsedJsonData);
        }

        public T GetHash<T>(string hashKey, string key, int databaseId = 0)
        {
            string parsedJsonData = _redisServer.GetDatabase(databaseId).HashGet(hashKey, key);
            return JsonConvert.DeserializeObject<T>(parsedJsonData);
        }

        public void RemoveHash(string hashKey, string key, int databaseId = 0)
        {
            _redisServer.GetDatabase(databaseId).HashDelete(hashKey, key);
        }
    }
}
