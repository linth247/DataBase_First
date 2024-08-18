using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Dtos;
using WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly WebContext _todoContext;
        //64.【12.身分驗證】ASP.NET Core Web API 入門教學(12_4) - JWT身分驗證
        private readonly IConfiguration _configuration;
        public LoginController(WebContext todoContext,
            IConfiguration configuration)
        { 
            _todoContext = todoContext;
            _configuration = configuration;
        }

        [HttpPost]
        //使用Cookie
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
                   new Claim("EmployeeId", user.EmployeeId.ToString())
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

        //使用jwt
        [HttpPost("jwtLogin")]
        public string jwtLogin(LoginPost value)
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
                //宣告

                //這邊等等寫驗證
                var claims = new List<Claim>
                {
                    //設定驗證成功
                    new Claim(JwtRegisteredClaimNames.Email, user.Account),
                    new Claim("FullName", user.Name), // 加上使用者的資訊
                   // new Claim(ClaimTypes.Role, "Administrator")
                   // new Claim(ClaimTypes.Role, "select")
                   //new Claim("EmployeeId", user.EmployeeId.ToString())
                   new Claim(JwtRegisteredClaimNames.NameId, user.EmployeeId.ToString()),
                   new Claim("EmployeeId", user.EmployeeId.ToString())
                };

                var role = from a in _todoContext.Role
                           where a.EmployeeId == user.EmployeeId
                           select a;

                foreach (var temp in role)
                {
                    //這個帳號，有哪些角色，一個一個加上去
                    claims.Add(new Claim(ClaimTypes.Role, temp.Name));
                    //claims.Add(new Claim(JwtRegisteredClaimNames.Sub, temp.Name));
                }

                //產生JWT
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

                var jwt = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"], // 發行者
                    audience: _configuration["JWT:Audience"], // 給誰使用
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30), // 期限
                    // 金鑰產生
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                return token;
            }
        }

        // 測試
        [AllowAnonymous]
        [HttpGet("jwt")]
        public IActionResult Get(string userName, string pwd)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    //頒發者
                    issuer: _configuration["JWT:Issuer"],
                    //接收者
                    audience: _configuration["JWT:Audience"],
                    //過期時間（可自行設定，注意和上面的claims內部Exp參數保持一致）
                    expires: DateTime.Now.AddMinutes(15),
                    //簽名證書
                    signingCredentials: creds,
                    //自定義參數
                    claims: claims
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return BadRequest(new { message = "帳號或密碼失敗" });
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
