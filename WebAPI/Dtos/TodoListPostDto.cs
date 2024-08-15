using WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.ValidationAttributes;

namespace WebAPI.Dtos
{
    //33.【5.新增資料POST】ASP.NET Core Web API 入門教學(5_8) - 資料驗證
    //https://learn.microsoft.com/zh-tw/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
    [StartEnd]
    [Test(Tvalue = "321")]
    public class TodoListPostDto
    {
        // 驗證錯誤訊息
        //"errors": {
        //     "Name": [
        //         "The Name field is required.",
        //         "The Name field is not a valid e-mail address."
        //         "名字請輸入電子信箱",
        //         "The field Name must be a string with a maximum length of 1.",
        //         "The field Name must match the regular expression '[a-z]'."
        //     ],
        //     "Orders": [
        //         "The field Orders must be between 2 and 3."
        //     ]
        // },

        //public Guid TodoId { get; set; }
        [Required]
        [TodoName]
        //[EmailAddress(ErrorMessage = "名字請輸入電子信箱")]
        //[StringLength(1)]
        //[RegularExpression("[a-z]")]
        public string Name { get; set; }
        //public DateTime InsertTime { get; set; }
        //public DateTime UpdateTime { get; set; }
        public bool Enable { get; set; }
        [Range(2,9)]
        public int Orders { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //public Guid InsertEmployeeId { get; set; }
        //public Guid UpdateEmployeeId { get; set; }

        //public string InsertEmployeeName { get; set; }
        //public string UpdateEmployeeName { get; set; }
        public ICollection<UploadFilePostDto> UploadFiles { get; set; }

    }
}
