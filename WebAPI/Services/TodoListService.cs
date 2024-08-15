using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Models;
using WebAPI.Parameters;

namespace WebAPI.Services
{
    public class TodoListService
    {
        private readonly WebContext _todoContext;
        private readonly IMapper _mapper;

        public TodoListService(WebContext todoContext, IMapper mapper)
        //public TodoController(WebContext todoContext)
        {
            _todoContext = todoContext;
            _mapper = mapper;
        }
        public TodoListService()
        {
            
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

            // 將DTO轉換部份函式化
            //return result.ToList().Select(a => ItemToDto(a));
            return result.ToList().Select(a => ItemToDto(a)).ToList();
        }

        public TodoListDto 取得單筆資料(Guid todoId)
        {
            var result = (from a in _todoContext.TodoList
                          where a.TodoId == todoId
                          select a)
              .Include(a => a.InsertEmployee)
              .Include(a => a.UpdateEmployee)
              .Include(a => a.UploadFiles)
              .SingleOrDefault();

            if (result != null)
            {
                return ItemToDto(result);
            }
            return null;
        }

        public IEnumerable<TodoListDto> 使用AutoMapper取得資料(TodoSelectParameter value)
        {
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
            return _mapper.Map<IEnumerable<TodoListDto>>(result);
        }


        private static TodoListDto ItemToDto(TodoList a)
        {
            List<UploadFileDto> updto = new List<UploadFileDto>();

            foreach (var temp in a.UploadFiles)
            {
                UploadFileDto up = new UploadFileDto
                {
                    Name = temp.Name,
                    Src = temp.Src,
                    TodoId = temp.TodoId,
                    UploadFileId = temp.UploadFileId
                };
                updto.Add(up);
            }


            return new TodoListDto
            {
                Enable = a.Enable,
                InsertEmployeeName = a.InsertEmployee.Name + "(" + a.InsertEmployeeId + ")",
                InsertTime = a.InsertTime,
                Name = a.Name,
                Orders = a.Orders,
                TodoId = a.TodoId,
                UpdateEmployeeName = a.UpdateEmployee.Name + "(" + a.UpdateEmployeeId + ")",
                UpdateTime = a.UpdateTime,
                UploadFiles = updto
                //InsertEmployeeId = a.InsertEmployeeId,
                //UpdateEmployeeId = a.UpdateEmployeeId,
            };
        }
    }
}
