using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using client.Models;
using server;
using client.Services;
using System.Net;

namespace client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CacheClient _cacheClient;

        public HomeController(ILogger<HomeController> logger, CacheClient cacheClient)
        {
            _logger = logger;
            _cacheClient = cacheClient;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Index()
        {
            var greeter = new GreeterClient();
            var result = await greeter.Greet();

            var actionResult = new ActionResult<string>(result);
            return await Task.FromResult(actionResult);
        }
        [HttpPost]
        public async Task<ActionResult<string>> Index([FromBody] LoginModel loginModel)
        {
            var isLocked = await _cacheClient.UpdateCache(loginModel.Username);
            return new ActionResult<string>($"is the user locked: {isLocked}");
        }

        public class LoginModel
        {
            public string Username { get; set; }
        }
    }
}
