using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EntityServer.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace EntityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthLogic _authLogic;

        public AuthController(AuthLogic authLogic)
        {
            _authLogic = authLogic;
        }


        [Route("signin")]
        [HttpGet, AllowAnonymous]
        public async Task<string> SignIn()
        {
            return await _authLogic.SignIn(HttpContext.Request.Headers["Authorization"][0]);
        }
    }
}
