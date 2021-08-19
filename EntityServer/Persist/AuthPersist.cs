using EntityServer.Contracts;
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
    }
}
