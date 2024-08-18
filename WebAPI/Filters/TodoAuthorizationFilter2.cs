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
        public string Roles = "";
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public TodoAuthorizationFilter2()
        {

        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //取得服務
            WebContext _todoContext= (WebContext)context.
                HttpContext.RequestServices.GetService(typeof(WebContext));

            IHttpContextAccessor _httpContextAccessor = (IHttpContextAccessor)context.
                HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));


            //var employeeid = _httpContextAccessor.HttpContext.User.FindFirst("EmployeeId").Value;

            //var Claim = _httpContextAccessor.HttpContext.User.Claims.ToList();
            //var employeeid = Claim.Where(a => a.Type == "EmployeeId").First().Value;

            var role =(from a in _todoContext.Role
                     where a.Name == Roles // 先撈全部
                     //&&  a.EmployeeId == // 還要加上是哪個使用者
                     select a).FirstOrDefault();

            // 有抓到select
            if (role == null)
            {
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
