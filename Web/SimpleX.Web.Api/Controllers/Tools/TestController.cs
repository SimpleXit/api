using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Web.Api.Controllers.Tools
{
    [Authorize]
    [Route("api/test")]
    [ApiController]
    public class TestController : SimpleController
    {
        public TestController(ILogger<TestController> logger) : base(logger)
        {

        }

        [HttpGet("authorised")]
        public async Task<ActionResult<string>> GetOnlyAuthorized()
        {
            _logger.LogInformation("TestController.GetOnlyAuthorized()");
            await Task.Delay(1);
            return Ok("Hello user");
        }

        [AllowAnonymous]
        [HttpGet("anonymous")]
        public async Task<ActionResult<string>> GetAnonymous()
        {
            _logger.LogInformation("TestController.GetAnonymous()");
            await Task.Delay(1);
            return Ok("Hello guest");
        }

        [AllowAnonymous]
        [HttpGet("badrequestonevensecond")]
        public async Task<ActionResult<string>> GetBadRequest()
        {
            await Task.Delay(1);

            if (DateTime.Now.Second % 2 == 0)
                return BadRequest();
            else
                return Ok("400");
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetWithID(long id)
        {
            await Task.Delay(1);
            return Ok(id.ToString());
        }

        [AllowAnonymous]
        [HttpGet("byValue/{value}")]
        public async Task<ActionResult<string>> GetWithString(string value)
        {
            await Task.Delay(1);
            return Ok(value);
        }

        [AllowAnonymous]
        [HttpGet("byIdAndValue")]
        public async Task<ActionResult<string>> GetWithIdAndString(long id, string value)
        {
            await Task.Delay(1);
            return Ok(value);
        }

        [AllowAnonymous]
        [HttpGet("error")]
        public async Task<ActionResult<string>> GetReturnsError(string value)
        {
            await Task.Delay(1);
            try
            {
                throw new ArgumentException("just some random exception");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
