using EntityServer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Logic.Contracts;

namespace EntityServer.Services
{
    public class HmacTokenService : ITokenService
    {
        private readonly IAuthPersist _authPersist;
        private readonly JsonTokenService _tokenService;
        private readonly IConfiguration _config;
        public HmacTokenService(IAuthPersist authPersist, JsonTokenService tokenService, IConfiguration config)
        {
            _authPersist = authPersist;
            _tokenService = tokenService;
            _config = config;
        }
        public async Task<string> GenerateToken(UserCredsModel userCreds, string apiName)
        {
            var apiConfig = _config.GetSection("Apis").Get<List<ApiConfigModel>>().Find(apm => apm.ApiName == apiName);
            if (apiConfig is null)
                throw new Exception("basic error has accoured");

            var tokenId = await _tokenService.GenerateToken(userCreds, apiName);
            var tag = Convert.ToBase64String(Logic.Services.CryptoService.ApplyHmac(tokenId, apiConfig.Salt));

            return tokenId + "." + tag;

        }

        public async Task TryAuthToken(string token, string apiName)
        {
            var isHasHmac = token.IndexOf(".") != -1;
            if (!isHasHmac)
                throw new Exception("basic error accoured");

            var apiConfig = _config.GetSection("Apis").Get<List<ApiConfigModel>>().Find(apm => apm.ApiName == apiName);

            if (apiConfig is null)
                throw new Exception("basic error has accoured");

            var tokenArr = token.Split(".");
            var tokenId = tokenArr[0];
            var hmacTag = tokenArr[1];
            var tag = Convert.ToBase64String(Logic.Services.CryptoService.ApplyHmac(tokenId.Substring("Bearer ".Length), apiConfig.Salt));
            var isCompare = CryptoService.VerifyHash(tag, hmacTag);

            if(!isCompare)
                throw new Exception("basic error has accoured");

            await _tokenService.TryAuthToken(tokenId,apiName);
        }
    }
}
