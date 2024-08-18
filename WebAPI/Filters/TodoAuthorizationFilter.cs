using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using WebAPI.Dtos;

namespace WebAPI.Filters
{
    public class TodoAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool tokenFlag = context.HttpContext.Request
                .Headers.TryGetValue("Authorization", out StringValues outValue);

            //當設定為全域驗證，要不受驗證邏輯的方法
            var ignore = (from a in context.ActionDescriptor.EndpointMetadata
                         where a.GetType () == typeof (AllowAnonymousAttribute)
                         select a).FirstOrDefault();

            if (ignore == null)
            {
                if (tokenFlag)
                {
                    if (outValue != "123")
                    {
                        context.Result = new JsonResult(new ReturnJson()
                        {
                            Data = "test1",
                            HttpCode = 401,
                            ErrorMessage = "沒有登入"
                        });
                    }
                }
                else
                {
                    context.Result = new JsonResult(new ReturnJson()
                    {
                        Data = "test2",
                        HttpCode = 401,
                        ErrorMessage = "沒有登入"
                    });
                }
            }



        }
    }
}
