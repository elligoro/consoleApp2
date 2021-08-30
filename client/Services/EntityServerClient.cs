using client.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Logic.Contracts;
using Newtonsoft.Json;

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
                var res = await HandleHttpClientResponse(HttpMethod.Post, new AuthenticationHeaderValue("Basic", authReq.PayLoad));
                authRes.Token = res;
            }
            catch(Exception ex)
            {
                throw;
            }
            return authRes;
        }
        public async Task<string> HandleHttpClientResponse(HttpMethod httpMethod, AuthenticationHeaderValue auth = null)
        {
            var client = _httpClient.CreateClient(_httpClientConfigName);
            var request = new HttpRequestMessage(httpMethod, _httpClientConfigUri);
            client.DefaultRequestHeaders.Authorization = auth;
            var res = (await client.SendAsync(request)).Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<ApiResponse>(res.Result);
            if (!model.IsSuccess)
                throw new Exception($"{model.StatusCode}");

            return model.DataModel;
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
