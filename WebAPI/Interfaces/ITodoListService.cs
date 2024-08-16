using WebAPI.Dtos;
using WebAPI.Parameters;

namespace WebAPI.Interfaces
{
    public interface ITodoListService
    {
        List<TodoListDto> 取得資料(TodoSelectParameter value);
    }
}
