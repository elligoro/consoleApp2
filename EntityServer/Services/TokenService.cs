using EntityServer.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EntityServer.Services
{
    public class TokenService : ITokenService
    {
        private readonly IAuthPersist _authPersist;
        public TokenService(IAuthPersist authPersist)
        {
            _authPersist = authPersist;
        }


        public async Task<string> GenerateToken(UserCredsModel userCreds, string apiName)
        {

            var id = CryptoService.GetRandomCryptoString();
            var hashedId = CryptoService.HashSHA256(id);
            var sub = userCreds.UserName;
            var exp = DateTime.UtcNow.AddMinutes(10);
            var tokenModel = new TokenPayloadModel
            {
                Tid = hashedId,
                Sub = sub,
                Exp = exp,
                Iss = apiName
            };
            await _authPersist.GenerateToken(tokenModel);

            return id;
        }

        public async Task TryAuthToken(string token, string apiName)
        {
            if (token.IndexOf("Bearer ") < 0)
                throw new Exception($"bad credentials. status: {HttpStatusCode.BadRequest}");

            var tokenId = token.Substring("Bearer ".Length);
            if (string.IsNullOrEmpty(tokenId))
                throw new Exception($"token not found: status {HttpStatusCode.NotFound}");

            var tokenIdHash = CryptoService.HashSHA256(tokenId);

            var tokenModel = await _authPersist.TryGetToken(tokenIdHash);
            if (tokenModel is null)
                throw new Exception($"Token does not exist. status: {HttpStatusCode.Unauthorized}");

            if (tokenModel.Exp < DateTime.UtcNow)
            {
                await _authPersist.RevokeToken(tokenIdHash);
                throw new Exception($"Token has expired. status: {HttpStatusCode.Unauthorized}");
            }
        }
    }
}
