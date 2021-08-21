using EntityServer.Contracts;
using EntityServer.Services;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EntityServer.Business
{
    public class AuthLogic
    {
        private readonly IAuthPersist _authPersist;
        public AuthLogic(IAuthPersist authPersist)
        {
            _authPersist = authPersist;
        }

        public async Task<string> SignIn(string auth)
        {
            var userCreds = GetUserCredModel(auth);
            if (!await ValidateSignIn(userCreds))
                throw new Exception($"missing or wrong user creds: status code: {HttpStatusCode.BadRequest}");

            return await GenerateToken(userCreds);
        }
        private async Task<string> GenerateToken(UserCredsModel userCreds)
        {

            var id = CryptoService.GetRandomCryptoString();
            var hashedId = CryptoService.HashSHA256(id);
            var sub = userCreds.UserName;
            var exp = DateTime.UtcNow.AddMinutes(10);
            var tokenModel = new TokenPayloadModel { 
                Tid = hashedId,
                Sub = sub,
                Exp = exp
            };
            try
            {
                await _authPersist.GenerateToken(tokenModel);
            }
            catch(Exception ex)
            {
                throw new Exception($"problem with saving token: {JsonConvert.SerializeObject(tokenModel)}. ex: {ex.Message}");
            }

            return id;
        }

        public async Task TryAuthToken(string token)
        {
            if (token.IndexOf("Bearer ") < 0)
                throw new Exception($"bad credentials. status: {HttpStatusCode.BadRequest}");

            var tokenId = token.Substring("Bearer ".Length);
            if (string.IsNullOrEmpty(tokenId))
                throw new Exception($"token not found: status {HttpStatusCode.NotFound}");

            var tokenIdHash = CryptoService.HashSHA256(tokenId);

            var tokenModel = await _authPersist.TryGetToken(tokenIdHash);
            if(tokenModel is null)
                throw new Exception($"Token does not exist. status: {HttpStatusCode.Unauthorized}");

            if (tokenModel.Exp < DateTime.UtcNow)
            {
                await _authPersist.RevokeToken(tokenIdHash);
                throw new Exception($"Token has expired. status: {HttpStatusCode.Unauthorized}");
            }
        }

        private UserCredsModel GetUserCredModel(string auth)
        {
            if (string.IsNullOrEmpty(auth) || !auth.Contains("Basic "))
                throw new WebException("missing creds error");

            var offset = "Basic ".Length;
            auth = auth.Substring(offset);
            var decodeAuth = Encoding.UTF8.GetString(Convert.FromBase64String(auth));

            if (!decodeAuth.Contains(":"))
                throw new WebException("missing creds error");

            var decodedAuthArr = decodeAuth.Split(":");
            var userName = decodedAuthArr[0];
            var password = decodedAuthArr[1];

            return new UserCredsModel {
                UserName = userName,
                Password = password
            };
        }

        private async Task<bool> ValidateSignIn(UserCredsModel userCreds)
        {


            var credsDb = await _authPersist.ValidateSignIn(userCreds.UserName);
            if (userCreds is null)
                throw new Exception($"userName or password do not match: {HttpStatusCode.NotFound}");

            return CryptoService.VerifyHash(CryptoService.HashSHA256(userCreds.Password), credsDb.Password);
        }
    }
}
