using System.ComponentModel.DataAnnotations;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.ValidationAttributes
{
    public class TestAttribute: ValidationAttribute
    {
        private string _tvalue;
        public string Tvalue = "de1";
        public TestAttribute(string value = "de") {
            _tvalue = value;
        }

        // 覆寫這個方法
        //46.【8.模型資料驗證】ASP.NET Core Web API 入門教學(8_2) - 自訂類別模型資料驗證標籤和傳值
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var st = (TodoListPostDto)value;

            return new ValidationResult(Tvalue, new string[] {"tvalue"});
        }
    }
}
