using client.Contracts;
using client.Services;
using HttpClinet;
using Logic.Contracts;
using Logic.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;

        public AuthLogic(CacheClient cacheClient, HttpClinetHandler httpClientService, IConfiguration config)
        {
            _cacheClient = cacheClient;
            _httpClientService = httpClientService;
            _config = config;
        }
        public async Task<ApiResponse> TryAuthenticate(HttpRequest req, HttpResponse res)
        {
            string token = string.Empty;
            var iss = _config.GetValue<string>("EntityClientConfig:Iss");
            var authHeader = req.Headers["Authorization"][0];
            var basicAuthTypeStr = "Basic "; 
            if(string.IsNullOrEmpty(authHeader)
               || (authHeader.IndexOf(basicAuthTypeStr) < 0))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized, "bad credentials");
            }
               
            var credsEnc = authHeader.Substring(basicAuthTypeStr.Length);
            var creds = Encoding.UTF8.GetString(Convert.FromBase64String(credsEnc));
            var credsArr = creds.Split(":");

            if (string.IsNullOrEmpty(creds) || credsArr.Length != 2) {
                throw new HttpResponseException(HttpStatusCode.Unauthorized, "bad credentials");
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
                token = (await _httpClientService.TryAuthenticate(iss, new AuthenticationHeaderValue("Basic", request.PayLoad))).Token;
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

        public async Task<ApiResponse> TryValidateToken(HttpRequest req, HttpResponse res)
        {
            var auth = req.Headers["Authorization"][0];
            var iss = req.Headers["x-iss"][0];

            var token = auth.Substring("Bearer ".Length);

            //if(token.Split(".").Length < 2 || string.IsNullOrEmpty(iss))
            //    throw new HttpResponseException(HttpStatusCode.Unauthorized, "bad credentials");

            var request = new Logic.Contracts.AuthResquest
            {
                PayLoad = token
            };

            try
            {
                await _httpClientService.TryValidateToken(iss, new AuthenticationHeaderValue("Bearer", request.PayLoad));
            }
            catch (HttpResponseException ex)
            {
                return new ApiResponse(false, (int)ex.StatusCode, ex.Description);
            }
            catch (Exception ex)
            {
                throw;
            }
            return new ApiResponse(true, (int)HttpStatusCode.OK, "Token Validated");
        }
    }
}
