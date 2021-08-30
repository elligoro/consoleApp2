using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("error/{statusCode}")]
        [HttpGet]
        public IActionResult HandleError(int statusCode)
        {
            return new ObjectResult("123");
        }
    }
}
