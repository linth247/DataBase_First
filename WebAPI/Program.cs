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
builder.Services.AddScoped<ITodoListService, TodoLinqService>();
builder.Services.AddScoped<ITodoListService, TodoAutoMapperService>();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
