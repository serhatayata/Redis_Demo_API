using Redis_Demo_API.Extensions;
using Redis_Demo_API.Services.Abstract;
using StackExchange.Redis;

namespace Redis_Demo_API.Services.Concrete
{
    public class RedisCacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _client;

        public RedisCacheService(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("RedisConfiguration:ConnectionString")?.Value;

            ConfigurationOptions options = new ConfigurationOptions()
            {
                EndPoints =
                {
                    connectionString
                },
                AbortOnConnectFail=false,
                AsyncTimeout=10000,
                ConnectTimeout=10000,
                KeepAlive=180
            };

            _client = ConnectionMultiplexer.Connect(options);
        }

        #region Get<T>
        public T Get<T>(string key) where T : class
        {
            string value = _client.GetDatabase().StringGet(key);

            return value.ToObject<T>();
        }
        #endregion
        #region Get
        public string Get(string key)
        {
            return _client.GetDatabase().StringGet(key);
        }
        #endregion
        #region GetAsync<T>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            string value = await _client.GetDatabase().StringGetAsync(key);
            return value.ToObject<T>();
        }
        #endregion
        #region Set
        public void Set(string key, string value)
        {
            _client.GetDatabase().StringSet(key, value);
        }
        #endregion
        #region Set<T>
        public void Set<T>(string key, T value) where T : class
        {
            _client.GetDatabase().StringSet(key, value.ToJson());
        }
        #endregion
        #region SetAsync
        public Task SetAsync(string key, object value)
        {
            return _client.GetDatabase().StringSetAsync(key, value.ToJson());
        }
        #endregion
        #region Set
        public void Set(string key, object value, TimeSpan expiration)
        {
            _client.GetDatabase().StringSet(key, value.ToJson(), expiration);
        }
        #endregion
        #region SetAsync with TimeSpan
        public Task SetAsync(string key, object value, TimeSpan expiration)
        {
            return _client.GetDatabase().StringSetAsync(key, value.ToJson(), expiration);
        }
        #endregion
        #region Remove
        public void Remove(string key)
        {
            _client.GetDatabase().KeyDelete(key);
        }
        #endregion
        #region ScanKeysAsync
        public async Task<List<string>> ScanKeysAsync(string match, string count)
        {
            var schemas = new List<string>();
            int nextCursor = 0;
            do
            {
                RedisResult redisResult = await _client.GetDatabase().ExecuteAsync("SCAN", nextCursor.ToString(), "MATCH", match, "COUNT", count);
                var innerResult = (RedisResult[])redisResult;

                nextCursor = int.Parse((string)innerResult[0]);
                List<string> resultLines = ((string[])innerResult[1]).ToList();
                schemas.AddRange(resultLines);
            }
            while (nextCursor != 0);

            return schemas;
        }
        #endregion

    }
}
