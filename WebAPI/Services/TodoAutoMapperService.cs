using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Models;
using WebAPI.Parameters;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class TodoAutoMapperService : ITodoListService
    {
        private readonly WebContext _todoContext;
        private readonly IMapper _mapper;

        public TodoAutoMapperService(WebContext todoContext, IMapper mapper)
        {
            _todoContext = todoContext;
            _mapper = mapper;
        }

        public List<TodoListDto> 取得資料(TodoSelectParameter value)
        {
            // 商業邏輯
            var result = _todoContext.TodoList
                .Include(a => a.InsertEmployee)
                .Include(a => a.UpdateEmployee)
                .Include(a => a.UploadFiles)
                .Select(a => a);

            if (!string.IsNullOrWhiteSpace(value.name))
            {
                result = result.Where(a => a.Name.IndexOf(value.name) > -1);
            }

            if (value.enable != null)
            {
                result = result.Where(a => a.Enable == value.enable);
            }

            if (value.InsertTime != null)
            {
                result = result.Where(a => a.InsertTime.Date == value.InsertTime);
            }

            if (value.minOrder != null && value.maxOrder != null)
            {
                result = result.Where(a => a.Orders >= value.minOrder && a.Orders <= value.maxOrder);
            }

            //var map = _mapper.Map<IEnumerable<TodoListDto>>(result);
            return _mapper.Map<IEnumerable<TodoListDto>>(result).ToList();
        }

 
    }
}
