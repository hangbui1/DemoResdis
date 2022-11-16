using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWinformRedis.Dictionary
{
    public class RedisDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private static ConnectionMultiplexer _cnn;
        private string _redisKey;
        public RedisDictionary(string redisKey)
        {
            _redisKey = redisKey;
            _cnn = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        }
        private IDatabase GetRedisDb()
        {
            return _cnn.GetDatabase();
        }
        private string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        private T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }
        public void Add(TKey key, TValue value)
        {
            GetRedisDb().HashSet(_redisKey, Serialize(key), Serialize(value));
        }
        public bool Remove(TKey key)
        {
            return GetRedisDb().HashDelete(_redisKey, Serialize(key));
        }       
        public ICollection<TValue> Values
        {
            get { return new Collection<TValue>(GetRedisDb().HashValues(_redisKey).Select(h => Deserialize<TValue>(h.ToString())).ToList()); }
        }
        public ICollection<TKey> Keys
        {
            get { return new Collection<TKey>(GetRedisDb().HashKeys(_redisKey).Select(h => Deserialize<TKey>(h.ToString())).ToList()); }
        }
    }
}
