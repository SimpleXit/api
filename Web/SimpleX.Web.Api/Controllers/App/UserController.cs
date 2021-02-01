using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleX.Domain.Models.App;
using SimpleX.Domain.Services.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Web.Api.Controllers.App
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : SimpleController
    {
        private readonly IAppUserService _appUserService;
        
        public UserController(ILogger<UserController> logger, IAppUserService appUserService) : base(logger)
        {
            _appUserService = appUserService ??
                throw new ArgumentNullException(nameof(appUserService));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginDto login)
        {
            try
            {
                _logger.LogTrace($"UserController.Login({login.Username}, {login.Password})");

                return await _appUserService.Login(login.Username, login.Password);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
