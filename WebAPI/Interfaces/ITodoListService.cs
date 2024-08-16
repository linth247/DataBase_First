using WebAPI.Dtos;
using WebAPI.Parameters;

namespace WebAPI.Interfaces
{
    public interface ITodoListService
    {
        string type { get; }
        List<TodoListDto> 取得資料(TodoSelectParameter value);
    }
}
