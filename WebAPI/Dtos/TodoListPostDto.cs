using WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.ValidationAttributes;
using WebAPI.Abstracts;

namespace WebAPI.Dtos
{
    //33.【5.新增資料POST】ASP.NET Core Web API 入門教學(5_8) - 資料驗證
    //https://learn.microsoft.com/zh-tw/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
    //[StartEnd]
    //[Test(Tvalue = "321")]
    public class TodoListPostDto: TodoListEditDtoAbstract
    {

    }
}
