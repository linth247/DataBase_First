using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Dtos;

namespace WebAPI.Filters
{
    public class FileLimitAttribute : Attribute, IResourceFilter
    {
        public long Size = 100000;
        public string extension = ".mp4";
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public FileLimitAttribute(long size=1000)
        {
            Size = size;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var files = context.HttpContext.Request.Form.Files;

            foreach (var temp in files)
            {
                if(temp.Length > (1024 * 1024 * Size))
                {
                    // 這邊就會回傳
                    context.Result = new JsonResult(new ReturnJson()
                    {
                        Data = "test1",
                        HttpCode = 400,
                        ErrorMessage = "檔案太大囉"
                    });
                }
                //if (Path.GetExtension(temp.FileName) != ".mp4")
                if (Path.GetExtension(temp.FileName) != extension)
                {
                    context.Result = new JsonResult(new ReturnJson()
                    {
                        Data = "test1",
                        HttpCode = 400,
                        ErrorMessage = "只允許上傳mp4"
                    });
                }
            }
        }
    }
}
