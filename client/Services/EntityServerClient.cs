using client.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace client.Services
{
    public class EntityServerClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _config;
        private readonly string _httpClientConfigName;
        private readonly string _httpClientConfigUri;
        public EntityServerClient(IHttpClientFactory httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClientConfigName = _config.GetValue<string>("EntityClientConfig:Name");
            _httpClientConfigUri = _config.GetValue<string>("EntityClientConfig:URI");
        }
        public async Task<AuthResponse> TryAuthenticate(AuthResquest authReq)
        {
            // https://www.youtube.com/watch?v=cwgck1k0YKU 
            AuthResponse authRes = new AuthResponse();
            try
            {
                var client = _httpClient.CreateClient(_httpClientConfigName);
                var request = new HttpRequestMessage(HttpMethod.Post, _httpClientConfigUri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authReq.PayLoad);
                var res = (await client.PostAsync(request.RequestUri, request.Content)).Content.ReadAsStringAsync();
                authRes.Token = res.Result;
            }
            catch(Exception ex)
            {
                throw;
            }
            return authRes;
        }
    }

    public class EntityClientConfig
    {   
        public string URI { get; set; }
        public string Name { get; set; }
        public string PublicKey {get; set;}
        public string PrivateKey { get; set; }
    }
}
