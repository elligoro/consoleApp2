using client.Contracts;
using client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace client.Business
{
    public class AuthLogic
    {
        private readonly CacheClient _cacheClient;
        private readonly EntityServerClient _httpClientService;

        public AuthLogic(CacheClient cacheClient, EntityServerClient httpClientService)
        {
            _cacheClient = cacheClient;
            _httpClientService = httpClientService;
        }
        public async Task<string> TryAuthenticate(HttpRequest req, HttpResponse res)
        {
            string token = string.Empty;
            var authHeader = req.Headers["Authorization"][0];
            var basicAuthTypeStr = "Basic "; 
            if(string.IsNullOrEmpty(authHeader)
               || (authHeader.IndexOf(basicAuthTypeStr) < 0))
            {
                res.StatusCode = (int)HttpStatusCode.Forbidden;
            }
               
            var credsEnc = authHeader.Substring(basicAuthTypeStr.Length);
            var creds = Encoding.UTF8.GetString(Convert.FromBase64String(credsEnc));
            var credsArr = creds.Split(":");

            if (string.IsNullOrEmpty(creds) || credsArr.Length != 2) {
                res.StatusCode = (int)HttpStatusCode.Forbidden;
            }

            var userName = credsArr[0];
            var password = credsArr[1];

            // see if cache exists of attemps
            if(await _cacheClient.UpdateCache(userName))
            {
                res.StatusCode = (int)HttpStatusCode.Forbidden;
                res.Headers["WWW-Authenticate"] = $"Basic realm=\"login\" error=\"user_locked\"";
            }
            

            AuthResquest request = new AuthResquest
            {
                PayLoad = credsEnc
            };

            // if not locked: try to authenticate with entity server
            token = (await _httpClientService.TryAuthenticate(request)).Token;

            return token;
        }
    }
}
