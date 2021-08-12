using Grpc.Net.Client;
using server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.Services
{
    public class CacheClient
    {
        public CacheClient()
        {

        }
        public async Task<bool> UpdateCache(string username)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Cache.CacheClient(channel);
            var reqMsg = new CacheReqMessage { Username = username };
            var res = await client.UpdateCacheAsync(reqMsg);

            return res.IsLocked;
        }
    }
}
