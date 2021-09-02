using EntityServer.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityServer.Services
{
    public class JsonTokenService : ITokenService
    {

        private readonly TokenService _tokenService;

        public JsonTokenService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> GenerateToken(UserCredsModel userCreds, string apiName)
        {
            var tokenModel = new TokenPayloadModel
            {
                Tid = _tokenService.GenerateRandomTokenId(), // should be a abstract class of Token store base class
                                                             // with this method (and other similar methods)
                Sub = userCreds.UserName,
                Exp = DateTime.UtcNow.AddMinutes(10),
                Iss = apiName
            };

            var jsonModel = JsonConvert.SerializeObject(tokenModel);
            var jsonModelBytes = Encoding.UTF8.GetBytes(jsonModel);

            return await Task.FromResult(Convert.ToBase64String(jsonModelBytes));
        }

        public async Task TryAuthToken(string token, string apiName)
        {
            _tokenService.ValidateAuthHeader(token);

            var jsonModelBytes = Convert.FromBase64String(token.Substring("Bearer ".Length));
            var jsonModel = Encoding.UTF8.GetString(jsonModelBytes);
            var model = JsonConvert.DeserializeObject<TokenPayloadModel>(jsonModel);

            if (model?.Iss != apiName ||
                model.Exp < DateTime.UtcNow)
                throw new Exception("token has expired");

            await Task.CompletedTask;
        }
    }
}
