﻿using EntityServer.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EntityServer.Persist
{
    public class AuthPersist: IAuthPersist
    {
        private readonly UserAuthDbContext _context;

        public AuthPersist(UserAuthDbContext context)
        {
            _context = context;
        }
        public async Task<UserCreds> ValidateSignIn(string userName)
        {
            return await _context.UsersCreds.Where(uc => uc.Username == userName).FirstAsync();
        }
        public async Task GenerateToken(TokenPayloadModel tokenModel)
        {
            await _context.Tokens.AddAsync(new Token {
                                                        Tid = tokenModel.Tid,
                                                        Sub = tokenModel.Sub,
                                                        Exp = tokenModel.Exp
                                                     });
            await _context.SaveChangesAsync();
        }

        public async Task<TokenPayloadModel> TryGetToken(string tokenIdHash)
        {
            TokenPayloadModel model = null;
            var tokenDb = await _context.Tokens.FindAsync(tokenIdHash);
            if(!(tokenDb is null))
                model = new TokenPayloadModel
                {
                    Tid = tokenDb.Tid,
                    Sub = tokenDb.Sub,
                    Exp = tokenDb.Exp
                };
            
            return model;
        }
        public async Task RevokeToken(string tokenIdHash, Token tokenEntity=null)
        {
            if(tokenEntity is null)
                tokenEntity = await _context.Tokens.FindAsync(tokenIdHash);

            if (tokenEntity is null)
                throw new Exception($"token not found. status: {HttpStatusCode.NotFound}");

            _context.Tokens.Remove(tokenEntity);
            await _context.SaveChangesAsync();
        }
    }
}
