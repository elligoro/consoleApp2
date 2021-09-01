using EntityServer.Contracts;
using EntityServer.Services;
using Logic.Infra;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EntityServer.Business
{
    public class AuthLogic
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthPersist _authPersist;
        public AuthLogic(ITokenService tokenService, IAuthPersist authPersist)
        {
            _tokenService = tokenService;
            _authPersist = authPersist;
        }

        public async Task<Logic.Contracts.ApiResponse> SignIn(string auth, string iss)
        {
            var userCreds = GetUserCredModel(auth);
            if (!await ValidateSignIn(userCreds))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "bad user credentials");


            var token = await _tokenService.GenerateToken(userCreds, iss);
            return new Logic.Contracts.ApiResponse(true, 200, token);
        }

        public async Task<Logic.Contracts.ApiResponse> TryAuthToken(string token, string iss)
        {
            await _tokenService.TryAuthToken(token, iss);
            return new Logic.Contracts.ApiResponse(true, 200, "");
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
            if (credsDb is null)
                throw new Exception($"userName or password do not match: {HttpStatusCode.NotFound}");

            return CryptoService.VerifyHash(CryptoService.HashSHA256(userCreds.Password), credsDb.Password);
        }
    }
}
