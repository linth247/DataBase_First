using System.ComponentModel.DataAnnotations;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.ValidationAttributes
{
    public class TodoNameAttribute: ValidationAttribute
    {
        // 覆寫這個方法
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            WebContext _todoContext = (WebContext)validationContext.GetService(typeof(WebContext));

            var name = (string)value;

            var findName = from a in _todoContext.TodoList
                           where a.Name == name
                           select a;

            // 更新的時候，要排除自己，二次過濾(因為put會有問題)
            var dto = validationContext.ObjectInstance;
            if (dto.GetType() == typeof(TodoListPutDto)) //TodoListPutDto 再做二次過濾
            {
                var dtoUpdate = (TodoListPutDto)dto;
                findName = findName.Where(a => a.TodoId != dtoUpdate.TodoId); // 把自己排除
            }

            if (findName.FirstOrDefault() != null)
            {
                return new ValidationResult("已存在相同的代辦事項");
            }
            return ValidationResult.Success;
        }
    }
}
