using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Dtos;

namespace WebAPI.Filters
{
    public class TodoResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var contextResult = context.Result as ObjectResult;

            if (context.ModelState.IsValid) // 驗證都沒有問題，這裡會是true
            {
                //傳到前端是固定同一個格式
                context.Result = new JsonResult(new ReturnJson()
                {
                    Data = contextResult.Value
                });
            }
            else
            {
                context.Result = new JsonResult(new ReturnJson()
                {
                    Error = contextResult.Value
                });
            }


        }
    }
}
