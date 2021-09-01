using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Contracts
{
    public interface ITokenService
    {
        Task<string> GenerateToken(UserCredsModel userCreds, string apiName);
        Task TryAuthToken(string token, string apiName);
    }
}
