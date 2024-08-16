using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Parameters;

namespace WebAPI.Services
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly WebContext _todoContext;

        public TodoListRepository(WebContext todoContext)
        {
            _todoContext = todoContext;
        }

        public IQueryable<TodoList> 取得資料()
        {
            var result = _todoContext.TodoList
                .Include(a => a.InsertEmployee)
                .Include(a => a.UpdateEmployee)
                .Include(a => a.UploadFiles)
                .Select(a => a);            

            return result;
        }        
    }
}
