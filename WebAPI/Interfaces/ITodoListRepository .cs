using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Dtos;
using WebAPI.Models;
using WebAPI.Parameters;

namespace WebAPI.Interfaces
{
    public interface ITodoListRepository
    {
        IQueryable<TodoList> 取得資料();
    }
}
