using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Todo1Controller : ControllerBase
    {
        private readonly WebContext2 _todoContext;

        //public Todo1Controller(WebContext todoContext, IMapper mapper)
        public Todo1Controller(WebContext2 todoContext)
        {
            _todoContext = todoContext;
            //_mapper = mapper;
        }

        // GET: api/<TodoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // 這邊建議不要用SQL, 儘量用Linq
        [HttpGet("GetSQL")]
        public IEnumerable<TodoList> GetSQL(string name)
        {
            string sql = "select * from todolist where 1=1";

            // sql撈出name關鍵字
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = sql + " and name like N'%" + name + "%'";
            }
            //update [TodoList] set name=N'去上課1' where [TodoId] = '450E22DE-F9C1-44E2-948B-2F8F734118CB'

            var result = _todoContext.TodoList.FromSqlRaw(sql);

            return result;

        }

        [HttpGet("GetSQLDto")]
        public IEnumerable<TodoListDto> GetSQLDto(string name)
        {
            string sql = @"SELECT [TodoId]
      ,a.[Name]
      ,[InsertTime]
      ,[UpdateTime]
      ,[Enable]
      ,[Orders]
      ,b.Name as [InsertEmployeeName]
      ,c.Name as [UpdateEmployeeName]
  FROM [Web].[dbo].[TodoList] a
  join Employee b on a.InsertEmployeeId=b.EmployeeId
  join Employee c on a.UpdateEmployeeId=c.EmployeeId where 1=1";

            // sql撈出name關鍵字
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = sql + " and name like N'%" + name + "%'";
            }
            //update [TodoList] set name=N'去上課1' where [TodoId] = '450E22DE-F9C1-44E2-948B-2F8F734118CB'

            var result = _todoContext.TodoListDto.FromSqlRaw(sql);
            //var result = _todoContext.ExecSQL<TodoListDto>(sql);

            return result;

        }

        // GET api/<TodoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TodoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TodoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
