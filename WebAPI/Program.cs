using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Todo.Services;
using WebAPI.Filters;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Profiles;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<WebContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WebDatabase")));
builder.Services.AddDbContext<WebContext2>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WebDatabase")));
//builder.Services.AddAutoMapper(typeof(UserRoleReMapperConfig));
builder.Services.AddAutoMapper(typeof(TodoListProfile));
builder.Services.AddAutoMapper(typeof(UploadFileProfile));

//
builder.Services.AddScoped<TodoListService>();

builder.Services.AddScoped<TestDIService>();
//每次注入時，都是一個新的實例
builder.Services.AddSingleton<SingletonService>();
//每個Request為同一個新的實例
builder.Services.AddScoped<ScopedService>();
//程式運行期間，只會有一個實例
builder.Services.AddTransient<TransientService>();

//53.【9.使用DI依賴注入功能】ASP.NET Core Web API 入門教學(9_5) - DI_IoC用法
//54.【9.使用DI依賴注入功能】ASP.NET Core Web API 入門教學(9_6) - 同Interface依賴注入多個實作
// 一個介面，兩種不同的實現方式
// 注入實現多個服務，然後讓Controller那邊，控制選擇使用哪一個實作，取得資料
// 例如：付款方式，就可以選擇使用哪種服務，哪種付款方式
builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoListService, TodoLinqService>();
builder.Services.AddScoped<ITodoListService, TodoAutoMapperService>();
builder.Services.AddScoped<ITodoListService, TodoListRService>();

//63.【12.身分驗證】ASP.NET Core Web API 入門教學(12_3) - 取得登入使用者資訊與內建or自己打造閒談
builder.Services.AddHttpContextAccessor();

// 設定 Cookie 式登入驗證，指定登入登出 Action
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//    {
//        //未登入時會自動導到這個網址
//        options.LoginPath = new PathString("/api/Login/NoLogin");
//        //options.LoginPath = "/Auth/Login";
//        //options.LogoutPath = "/Auth/Logout";
//        //options.AccessDeniedPath = "/Auth/AccessDenied";
//        //沒有權限時，會自動導到這個網址
//        options.AccessDeniedPath = new PathString("/api/Login/NoAccess");
//        // 全部的cooike都會受影響
//        //options.ExpireTimeSpan=TimeSpan.FromSeconds(5); // 登入多久會失效

//    });

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"], // 發行者，一定是todo.com
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["Jwt:Audience"], // 接收者，一定是my, 才會過
//        ValidateLifetime = true, // 到期時間，就不能讀，預設true
//        // 金鑰
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]))
//    };
//});


//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(o =>
//{
//    o.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidIssuer = builder.Configuration["JWT:Issuer"],
//        ValidAudience = builder.Configuration["JWT:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey
//        (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = false,
//        ValidateIssuerSigningKey = true
//    };
//});

//builder.Services.AddAuthorization();

// 全部的controller 的API, 都必須受登入的控制，驗證才能使用
builder.Services.AddMvc(options =>
{
    //options.Filters.Add(new AuthorizeFilter());
    options.Filters.Add(new TodoAuthorizationFilter());
});


builder.Services.AddScoped<AsyncService>();
builder.Services.AddScoped<TodoListAsyncService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// 啟用身分認證
//app.UseCookiePolicy();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
