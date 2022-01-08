namespace Transfer.Common.Cache
{
    /// <summary>
    /// интерфейс хранения в кэше
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// достать из кэша
        /// </summary>
        T Get<T>(string cacheKey) where T : class;

        /// <summary>
        /// положить в кэш
        /// </summary>
        void Set<T>(string cacheKey, T data) where T : class;
    }
}
