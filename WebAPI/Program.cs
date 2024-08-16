using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<ITodoListService, TodoLinqService>();
builder.Services.AddScoped<ITodoListService, TodoAutoMapperService>();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
