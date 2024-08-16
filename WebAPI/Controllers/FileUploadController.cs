using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly WebContext _todoContext;
        private readonly IWebHostEnvironment _env;
        public FileUploadController(WebContext todoContext, IWebHostEnvironment env)
        {
            _todoContext = todoContext;
            _env = env;
        }
        //[HttpPost]
        //public void Post(IFormFileCollection files)
        //{
        //    var rootPath = _env.ContentRootPath + @"\wwwroot\";
        //}

        //56.【10.上傳檔案API】ASP.NET Core Web API 入門教學(10_1) - 基本上傳檔案
        //[HttpPost("From")]
        //[HttpPost("{id}")]
        [HttpPost]
        public async Task<IActionResult> Post(IFormFileCollection files, [FromForm] Guid id)
        {
            //var rootPath = _env.ContentRootPath + @"\\wwwroot\\";
            var rootPath = _env.ContentRootPath + @"\wwwroot\UploadFiles\"+id+"\\";

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = file.FileName;
                    using (var stream = System.IO.File.Create(rootPath + filePath))
                    {
                        //file.CopyTo(stream);
                        await file.CopyToAsync(stream);

                        var insert = new UploadFile
                        {
                            Name = filePath,
                            Src = "/UploadFiles/" + id + "/" + filePath,
                            TodoId = id
                        };

                        _todoContext.UploadFile.Add(insert);
                    }
                }
            }
            _todoContext.SaveChanges();

            return Ok(new { count = files.Count });
        }
    }
}
