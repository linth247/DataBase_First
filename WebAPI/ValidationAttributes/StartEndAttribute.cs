using System.ComponentModel.DataAnnotations;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.ValidationAttributes
{
    public class StartEndAttribute: ValidationAttribute
    {
        // 覆寫這個方法
        //46.【8.模型資料驗證】ASP.NET Core Web API 入門教學(8_2) - 自訂類別模型資料驗證標籤和傳值
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var st = (TodoListPostDto)value;

            if(st.StartTime >= st.EndTime)
            {
                return new ValidationResult("開始時間不可以大於結束時間", new string[] { "time" });
            }

            return ValidationResult.Success;
        }
    }
}
