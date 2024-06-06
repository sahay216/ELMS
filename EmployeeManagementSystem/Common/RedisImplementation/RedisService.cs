using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace Common.RedisImplementation
{
    public class RedisService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public void SetValue (RedisKey key, object  value, TimeSpan? absoluteExpirationRelativeToNow= null)
        {
            var options = new DistributedCacheEntryOptions();
            if (absoluteExpirationRelativeToNow.HasValue)   
            {
                options.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow.Value;
            }
            _cache. SetString(key.ToString(), JsonConvert.SerializeObject(value), options);
        }

        public T GetValue<T>(RedisKey key)
        {
            var serializedValue = _cache.GetString(key.ToString());
            if(serializedValue == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(serializedValue);
        }

        public void DeleteString(RedisKey key)
        {
            _cache.Remove(key.ToString());
        }
    }



    public enum RedisKey
    {
        UserID,
        NewHires,
        BirthdayEmployee,

    }
}
