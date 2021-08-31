using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Logic.Contracts;
using Newtonsoft.Json;
using Logic.Infra;
using System.Net;

namespace HttpClinet
{
    public class HttpClinetHandler
    {

        private readonly IHttpClientFactory _httpClient;
        private readonly string _name;
        private readonly string _uri;
        public HttpClinetHandler(IHttpClientFactory httpClient, string name, string uri)
        {
            _httpClient = httpClient;
            _name = name;
            _uri = uri;
        }
        public async Task<AuthResponse> TryAuthenticate(AuthenticationHeaderValue auth = null)
        {
            // https://www.youtube.com/watch?v=cwgck1k0YKU 
            AuthResponse authRes = new AuthResponse();
            try
            {
                var res = await HandleHttpClientCall(HttpMethod.Post, auth);
                authRes.Token = res;
            }
            catch (Exception ex)
            {
                var res = JsonConvert.DeserializeObject<ApiResponse>(ex.Message);
                throw new HttpResponseException((HttpStatusCode)res.StatusCode, res.DataModel);
            }
            return authRes;
        }
        private async Task<string> HandleHttpClientCall(HttpMethod httpMethod, AuthenticationHeaderValue auth = null)
        {
            var client = _httpClient.CreateClient(_name);
            var request = new HttpRequestMessage(httpMethod, _uri);
            request.Headers.Authorization = auth;
            var res = (await client.SendAsync(request)).Content.ReadAsStringAsync();
            var model = MapResponseContent(res.Result);
            return model;
        }

        private string MapResponseContent(string res)
        {
            var model = JsonConvert.DeserializeObject<ApiResponse>(res);
            if (!model.IsSuccess)
                throw new Exception(JsonConvert.SerializeObject(new ApiResponse(false, model.StatusCode, model.DataModel)));

            return model.DataModel;
        }
    }

    public class EntityClientConfig
    {
        public string URI { get; set; }
        public string Name { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

}
