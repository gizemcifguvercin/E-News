using System;
using System.Collections.Generic;
using System.Text;

namespace RedisCacheService.Configuration
{
    /// <summary>
    /// Redis server configuration informations
    /// </summary>
    public class RedisConfiguration
    {
        /// <summary>
        /// Redis server hostname
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Redis server password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Redis server port
        /// </summary>
        public int Port { get; set; }
    }
}
