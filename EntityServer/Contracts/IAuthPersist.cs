using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Contracts
{
    public interface IAuthPersist
    {
        Task<UserCreds> ValidateSignIn(string userName);
        Task GenerateToken(TokenPayloadModel tokenModel);
        Task<TokenPayloadModel> TryGetToken(string tokenIdHash);
        Task RevokeToken(string tokenIdHash, Token tokenEntity = null);
    }
}
