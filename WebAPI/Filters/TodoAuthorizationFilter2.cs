using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Filters
{
    public class TodoAuthorizationFilter2 : Attribute, IAuthorizationFilter
    {
        public string Roles="";
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public TodoAuthorizationFilter2()
        {

        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine("Roles:"+ Roles);

            bool tokenFlag = context.HttpContext.Request
                .Headers.TryGetValue("Authorization", out StringValues outValue);

            //取得服務
            WebContext _todoContext= (WebContext)context.
                HttpContext.RequestServices.GetService(typeof(WebContext));

            IHttpContextAccessor _httpContextAccessor = (IHttpContextAccessor)context.
                HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));


            //// 還要加上是哪個使用者
            //var employeeid = _httpContextAccessor.HttpContext.User.FindFirst("EmployeeId").Value;

            //var Claim = _httpContextAccessor.HttpContext.User.Claims.ToList();
            //var employeeid = Claim.Where(a => a.Type == "EmployeeId").First().Value;

            //當設定為全域驗證，要不受驗證邏輯的方法
            var ignore = (from a in context.ActionDescriptor.EndpointMetadata
                          where a.GetType() == typeof(AllowAnonymousAttribute)
                          select a).FirstOrDefault();

            if (ignore == null)
            {

                var role = (from a in _todoContext.Role
                            where a.Name == Roles // 先撈全部
                            //&&  a.EmployeeId == // 還要加上是哪個使用者
                            select a).FirstOrDefault();

                if (role == null)
                {
                    // 沒有抓到select
                    context.Result = new JsonResult(new ReturnJson()
                    {
                        Data = Roles,
                        HttpCode = 401,
                        ErrorMessage = "沒有登入"
                    });
                }
            }
        }
    }
}
