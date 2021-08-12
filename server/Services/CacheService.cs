using Grpc.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server
{
    public class CacheService : Cache.CacheBase
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;
        private readonly IConfiguration _config;
        private int _lockMinConfig;
        private int _startTryMinConfig;
        private short _maxTryLogin;

        public CacheService(IDistributedCache cache, ILogger<CacheService> logger, IConfiguration config)
        {
            _cache = cache;
            _logger = logger;
            _config = config;
            _lockMinConfig = _config.GetValue<int>("Cache:LockMin");
            _startTryMinConfig = _config.GetValue<int>("Cache:StartTryMin");
            _maxTryLogin = _config.GetValue<short>("Cache:MaxTryLogin");
        }

        private bool IsLocked(CacheModel model, int lockMin, DateTime now) => !(model is null) && model.LockTimeStart.HasValue
                                                                                         && model.LockTimeStart.Value.AddMinutes(lockMin) > now;

        private bool IsTryTimeSpan(CacheModel model, int tryTimeMin, DateTime now) => !(model is null) && model.CountTimeStart.HasValue
                                                                                                    && model.CountTimeStart.Value.AddMinutes(tryTimeMin) > now;

        public async override Task<CacheResMessage> GetCache(CacheReqMessage request, ServerCallContext context)
        {
            try
            {
                var userCache = await _cache.GetStringAsync(request.Username);
                var now = DateTime.UtcNow;
                return new CacheResMessage { IsLocked = IsLocked(JsonConvert.DeserializeObject<CacheModel>(userCache), _lockMinConfig, now) };
            }
            catch(Exception ex)
            {
                _logger.LogDebug($"GetCache for userName {request.Username}: {ex.Message}", ex.StackTrace);
                throw;
            }

        }

        public async override Task<CacheResMessage> UpdateCache(CacheReqMessage request, ServerCallContext context)
        {
            try
            {
                var userTryCacheRes = await _cache.GetStringAsync(request.Username);
                var now = DateTime.UtcNow;
                var model = UpdateCounter(userTryCacheRes, _lockMinConfig , _startTryMinConfig, _maxTryLogin, now);
                await _cache.SetStringAsync(request.Username, JsonConvert.SerializeObject(model));
                return new CacheResMessage { IsLocked = IsLocked(JsonConvert.DeserializeObject<CacheModel>(userTryCacheRes), _lockMinConfig, now) };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateCache: error: {ex.Message}");
                throw;
            }
        }

        protected CacheModel UpdateCounter(string modelStr, int lockMinConfig, int startTryMinConfig, short maxTryLogin,  DateTime now)
        {
            bool isLocked = false;
            bool isTryTimeSpan = false;
            CacheModel model;
            if (!string.IsNullOrEmpty(modelStr))
            {
                model = JsonConvert.DeserializeObject<CacheModel>(modelStr);
                isLocked = IsLocked(model, lockMinConfig, now);
                isTryTimeSpan = IsTryTimeSpan(model, startTryMinConfig, now);
            }
            else
                model = new CacheModel();

            if (string.IsNullOrEmpty(modelStr) || !isLocked) // locked
            {
                if (!isTryTimeSpan) // if not locked and time span not up to date or not exists -> make new record
                {
                    model.Count = 1;
                    model.CountTimeStart = now;
                    model.LockTimeStart = null;
                }
                else
                {
                    model.Count++;
                    if (model.Count >= maxTryLogin)
                        model.LockTimeStart = now;
                }
            }

            return model;
        }
    }

    public class CacheModel
    {
        public int Count { get; set; }
        public DateTime? CountTimeStart { get; set; }
        public DateTime? LockTimeStart { get; set; }
    }
}
