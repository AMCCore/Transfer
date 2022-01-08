using System.Runtime.Caching;

namespace Transfer.Common.Cache
{
    /// <summary>
    /// реализация (MemoryCache) кэша 
    /// </summary>
    public class InMemoryCache : ICacheService
    {
        private readonly int interval;

        public InMemoryCache()
        {
            interval = 10;
        }

        public InMemoryCache(int interval)
        {
            this.interval = interval;
        }

        /// <summary>
        /// достать из кэша
        /// </summary>
        public T Get<T>(string cacheKey) where T : class
        {
            var item = MemoryCache.Default.Get(cacheKey) as T;
            return item;
        }

        /// <summary>
        /// положить в кэш
        /// </summary>
        public void Set<T>(string cacheKey, T data) where T : class
        {
            MemoryCache.Default.Remove(cacheKey);
            MemoryCache.Default.Add(cacheKey, data, DateTime.Now.AddMinutes(interval));
        }
    }
}
