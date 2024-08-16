using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }
        //56.【10.上傳檔案API】ASP.NET Core Web API 入門教學(10_1) - 基本上傳檔案
        [HttpPost]
        public void Post(ICollection<IFormFile> files)
        {
            var rootPath = _env.ContentRootPath + @"\\wwwroot\\";

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = file.FileName;
                    using (var stream = System.IO.File.Create(rootPath + filePath))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
        }

    }
}
