using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Parameters;

namespace Todo.Services
{
    public class TodoListRService : ITodoListService
    {
        private readonly ITodoListRepository _todoListRepository;

        public string type => "fun";

        public TodoListRService(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public List<TodoListDto> 取得資料(TodoSelectParameter value)
        {
            var result = _todoListRepository.取得資料();


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

            return result.ToList().Select(a => ItemToDto(a)).ToList();
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
                InsertEmployeeName = a.InsertEmployee.Name,
                InsertTime = a.InsertTime,
                Name = a.Name + "(use Repository)",
                Orders = a.Orders,
                TodoId = a.TodoId,
                UpdateEmployeeName = a.UpdateEmployee.Name,
                UpdateTime = a.UpdateTime,
                UploadFiles = updto
            };
        }
    }

}
