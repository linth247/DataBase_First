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
                   // new Claim(ClaimTypes.Role, "select")
                };

                var role = from a in _todoContext.Role
                           where a.EmployeeId == user.EmployeeId
                           select a;

                foreach (var temp in role)
                {
                    //這個帳號，有哪些角色，一個一個加上去
                    claims.Add(new Claim(ClaimTypes.Role, temp.Name));
                }

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(4)

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };


                // 原理
                var claimsIdentity = new ClaimsIdentity(
                    claims, 
                    CookieAuthenticationDefaults.AuthenticationScheme);
                // 狀態控制
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), 
                    authProperties);
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
        
        [HttpGet("NoAccess")]
        public string noAccess()
        {
            return "沒有權限";
        }

    }
}
