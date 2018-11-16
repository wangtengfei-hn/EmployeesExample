using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Redis.Repository
{
    public class RedisContext
    {
        private PooledRedisClientManager RedisManager { get; set; }

        public RedisContext(string server, int port, string password)
        {
            this.RedisManager = new PooledRedisClientManager(
                new string[] { string.Format("{2}@{0}:{1}", server, port, password) },
                new string[] { string.Format("{2}@{0}:{1}", server, port, password) },
                new RedisClientManagerConfig()
                {
                    MaxWritePoolSize = 1000,
                    MaxReadPoolSize = 1000,
                    AutoStart = true
                },
                0,
                20000,
                5);
        }
        public RedisContext(IEnumerable<string> readWriteHosts, IEnumerable<string> readOnlyHosts)
        {
            this.RedisManager = new PooledRedisClientManager(
                readWriteHosts,
                readOnlyHosts,
                new RedisClientManagerConfig()
                {
                    MaxWritePoolSize = 1000,
                    MaxReadPoolSize = 1000,
                    AutoStart = true
                },
                0,
                20000,
                5);
        }

        #region 根据Key操作

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            using (var redisClient = this.RedisManager.GetReadOnlyClient())
            {
                var result = redisClient.Get<T>(key);

                return result;
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="action">当获取不到数据时执行</param>
        /// <returns></returns>
        public T Get<T>(string key, Func<T> action = null)
        {
            using (var redisClient = this.RedisManager.GetClient())
            {
                var result = redisClient.Get<T>(key);
                if (((typeof(T).IsClass && result == null) || (result.Equals(default(T)))) && action != null)
                {
                    result = action();
                    if (result != null)
                    {
                        redisClient.Set<T>(key, result);
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {
            if (value != null)
            {
                using (var redisClient = this.RedisManager.GetClient())
                {
                    redisClient.Set<T>(key, value);
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            using (var redisClient = this.RedisManager.GetClient())
            {
                return redisClient.Remove(key);
            }
        }
        /// <summary>
        /// 是否存在Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            using (var redisClient = this.RedisManager.GetReadOnlyClient())
            {
                return redisClient.ContainsKey(key);
            }
        }
        #endregion

        #region 根据Hash和Key操作

        private T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }
        private string Serialize<T>(T value)
        {
            if (value == null)
                return null;
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string hashId, string key)
        {
            using (var redisClient = this.RedisManager.GetReadOnlyClient())
            {
                var result = Deserialize<T>(redisClient.GetValueFromHash(hashId, key));
                return result;
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="action">当获取不到数据时执行</param>
        /// <returns></returns>
        public T Get<T>(string hashId, string key, Func<T> action = null)
        {
            using (var redisClient = this.RedisManager.GetClient())
            {
                var result = Deserialize<T>(redisClient.GetValueFromHash(hashId, key));
                if (((typeof(T).IsClass && result == null) || (result.Equals(default(T)))) && action != null)
                {
                    result = action();
                    if (result != null)
                    {
                        redisClient.SetEntryInHash(hashId, key.ToString(), Serialize(result));
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string hashId, string key, T value)
        {
            if (value != null)
            {
                using (var redisClient = this.RedisManager.GetClient())
                {
                    redisClient.SetEntryInHash(hashId, key.ToString(), Serialize(value));
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string hashId, string key)
        {
            using (var redisClient = this.RedisManager.GetClient())
            {
                return redisClient.RemoveEntryFromHash(hashId, key);
            }
        }
        /// <summary>
        /// 是否存在Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string hashId, string key)
        {
            using (var redisClient = this.RedisManager.GetReadOnlyClient())
            {
                return redisClient.HashContainsEntry(hashId, key);
            }
        }

        /// <summary>
        /// 获取/创建自增列
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="incrementBy">自增量</param>
        /// <returns></returns>
        public long GetIncremenValueFormHash(string hashId, string key, int incrementBy = 0)
        {
            using (var redisClient = this.RedisManager.GetClient())
            {
                return redisClient.IncrementValueInHash(hashId, key.ToString(), incrementBy);
            }
        }
        #endregion

    }
}
