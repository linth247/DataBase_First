using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
