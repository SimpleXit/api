using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleX.Web.Api.Models;
using SimpleX.Web.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Web.Api.Controllers.Tools
{
    [Authorize]
    [ApiController]
    [Route("api/files")]
    public class FileController : SimpleController
    {
        private readonly FileService _fileService;

        public FileController(ILogger<FileController> logger, FileService service) : base(logger)
        {
            _fileService = service;
        }

        //public IActionResult UploadFiles([FromForm(Name = "files")] List<IFormFile> files, string subDirectory)
        [HttpPost("upload")]
        public async Task<ActionResult> UploadFiles([FromForm(Name = "files")] List<IFormFile> files, string subDirectory)
        {
            try
            {
                await _fileService.SaveFiles(files, subDirectory);

                return Ok(new UploadResult() { FileCount = files.Count.ToString(), TotalSize = FileService.SizeConverter(files.Sum(f => f.Length)) });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

    }
}
