using client.Contracts;
using client.Services;
using HttpClinet;
using Logic.Contracts;
using Logic.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace client.Business
{
    public class AuthLogic
    {
        private readonly CacheClient _cacheClient;
        private readonly HttpClinetHandler _httpClientService;

        public AuthLogic(CacheClient cacheClient, HttpClinetHandler httpClientService)
        {
            _cacheClient = cacheClient;
            _httpClientService = httpClientService;
        }
        public async Task<ApiResponse> TryAuthenticate(HttpRequest req, HttpResponse res)
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
            

            var request = new Logic.Contracts.AuthResquest
            {
                PayLoad = credsEnc
            };
            try
            {
                // if not locked: try to authenticate with entity server
                token = (await _httpClientService.TryAuthenticate(new AuthenticationHeaderValue("Basic", request.PayLoad))).Token;
            }
            catch(HttpResponseException ex)
            {
                return new ApiResponse(false, (int)ex.StatusCode,ex.Description);
            }
            catch (Exception ex)
            {
                throw;
            }
            return new ApiResponse(true, (int)HttpStatusCode.OK, token);
        }
    }
}
