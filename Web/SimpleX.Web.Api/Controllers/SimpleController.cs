using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Web.Api.Controllers
{
    public abstract class SimpleController : ControllerBase
    {
        protected readonly ILogger _logger;

        public SimpleController(ILogger logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        protected ActionResult Error(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem(title: ex.Message, detail: ex.StackTrace, statusCode: 500);
        }
    }
}
