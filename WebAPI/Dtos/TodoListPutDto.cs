using WebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    //33.【5.新增資料POST】ASP.NET Core Web API 入門教學(5_8) - 資料驗證
    //https://learn.microsoft.com/zh-tw/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
    public class TodoListPutDto
    {
        //36.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_2) - 使用DTO更新資和架構思考
        public Guid TodoId { get; set; }
        public string Name { get; set; }

        public bool Enable { get; set; }
        [Range(2,9)]
        public int Orders { get; set; }

        public ICollection<UploadFilePostDto> UploadFiles { get; set; }

        public TodoListPutDto() { 
            UploadFiles = new List<UploadFilePostDto>();
        }
    }
}
