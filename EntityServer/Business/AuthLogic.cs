using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Headers;
using EntityServer.Contracts;
using EntityServer.Services;
using System.Security.Cryptography;

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

            return await GenerateToken();
        }
        private async Task<string> GenerateToken()
        {
            return await Task.FromResult("Token");

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
