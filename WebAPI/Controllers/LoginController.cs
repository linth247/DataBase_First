using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Dtos;
using WebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly WebContext _todoContext;
        public LoginController(WebContext todoContext)
        { 
            _todoContext = todoContext;
        }

        [HttpPost]
        public string login(LoginPost value)
        {
            var user = (from a in _todoContext.Employee
                        where a.Account == value.Account
                        && a.Password == value.Password
                        select a).SingleOrDefault();

            if (user == null)
            {
                return "帳號密碼錯誤";
            }
            else
            {
                //這邊等等寫驗證
                var claims = new List<Claim>
                {
                    //設定驗證成功
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim("FullName", user.Name), // 加上使用者的資訊
                   // new Claim(ClaimTypes.Role, "Administrator")
                };
                // 原理
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // 狀態控制
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return "ok";
            }
        }

        [HttpDelete]
        public void logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        [HttpGet("NoLogin")]
        public string noLogin()
        {
            return "未登入";
        }

    }
}
