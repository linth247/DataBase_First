using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
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
//�C���`�J�ɡA���O�@�ӷs�����
builder.Services.AddSingleton<SingletonService>();
//�C��Request���P�@�ӷs�����
builder.Services.AddScoped<ScopedService>();
//�{���B������A�u�|���@�ӹ��
builder.Services.AddTransient<TransientService>();

//53.�i9.�ϥ�DI�̿�`�J�\��jASP.NET Core Web API �J���о�(9_5) - DI_IoC�Ϊk
//54.�i9.�ϥ�DI�̿�`�J�\��jASP.NET Core Web API �J���о�(9_6) - �PInterface�̿�`�J�h�ӹ�@
// �@�Ӥ����A��ؤ��P����{�覡
// �`�J��{�h�ӪA�ȡA�M����Controller����A�����ܨϥέ��@�ӹ�@�A���o���
// �Ҧp�G�I�ڤ覡�A�N�i�H��ܨϥέ��تA�ȡA���إI�ڤ覡
builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoListService, TodoLinqService>();
builder.Services.AddScoped<ITodoListService, TodoAutoMapperService>();
builder.Services.AddScoped<ITodoListService, TodoListRService>();

//63.�i12.�������ҡjASP.NET Core Web API �J���о�(12_3) - ���o�n�J�ϥΪ̸�T�P����or�ۤv���y����
builder.Services.AddHttpContextAccessor();

//-------------------------------------
// Cookie����
// �]�w Cookie ���n�J���ҡA���w�n�J�n�X Action
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//    {
//        //���n�J�ɷ|�۰ʾɨ�o�Ӻ��}
//        options.LoginPath = new PathString("/api/Login/NoLogin");
//        //options.LoginPath = "/Auth/Login";
//        //options.LogoutPath = "/Auth/Logout";
//        //options.AccessDeniedPath = "/Auth/AccessDenied";
//        //�S���v���ɡA�|�۰ʾɨ�o�Ӻ��}
//        options.AccessDeniedPath = new PathString("/api/Login/NoAccess");
//        // ������cooike���|���v�T
//        //options.ExpireTimeSpan=TimeSpan.FromSeconds(5); // �n�J�h�[�|����

//    });

//-------------------------------------
//JWT����(���ت�)
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,   // ���ҵo���
//        ValidIssuer = builder.Configuration["Jwt:Issuer"], // �o��̡A�@�w�Otodo.com
//        ValidateAudience = true, // ���ұ�����
//        ValidAudience = builder.Configuration["Jwt:Audience"], // �����̡A�@�w�Omy, �~�|�L
//        ValidateLifetime = true, // ����ɶ��A�N����Ū�A�w�]true
//        ClockSkew = TimeSpan.Zero, //�ǽT��Ӯɶ��L��
//        // ���_
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]))
//    };
//});

// �����W��
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

//builder.Services.AddAuthorization();  // �o�ӥi�H����

// ������controller ��API, ���������n�J������A���Ҥ~��ϥ�
builder.Services.AddMvc(options =>
{
    //options.Filters.Add(new AuthorizeFilter());
    //options.Filters.Add(new TodoAuthorizationFilter());
    //options.Filters.Add(new TodoAuthorizationFilter2());
    //options.Filters.Add(typeof(TodoAuthorizationFilter2));
    options.Filters.Add(typeof(TodoActionFilter)); // ���غc�l�i�H�o�˼g
    options.Filters.Add(typeof(TodoResultFilter)); // ����ResultFilter
});


builder.Services.AddScoped<AsyncService>();
builder.Services.AddScoped<TodoListAsyncService>();


//builder.WebHost.UseKestrel(options =>
//{
//    options.Limits.MaxRequestLineSize = 10 * 1024 * 1024;
//    options.Limits.MaxRequestBufferSize = 10 * 1024 * 1024;
//    //options.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
//    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
//});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueCountLimit = 10;
    x.ValueLengthLimit = int.MaxValue;
    //x.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
    x.MultipartBodyLengthLimit = long.MaxValue; // 10MB
    x.MemoryBufferThreshold = 10 * 1024 * 1024; // 1MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// �ҥΨ����{��
//app.UseCookiePolicy();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
