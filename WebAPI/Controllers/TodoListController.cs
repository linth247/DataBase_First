using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Dtos;
using WebAPI.Parameters;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Data.SqlClient;
//using AutoMapper;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly WebContext _todoContext;
        private readonly IMapper _mapper;

        public TodoController(WebContext todoContext, IMapper mapper)
        //public TodoController(WebContext todoContext)
        {
            _todoContext = todoContext;
            _mapper = mapper;
        }

        // GET: api/<TodoController>
        [HttpGet]
        //public IEnumerable<TodoListDto> Get([FromQuery] TodoSelectParameter value)
        public IActionResult Get([FromQuery] TodoSelectParameter value)
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

            if (result == null || result.Count() <=0)
            {
                return NotFound("找不到資源");
            }

            // 將DTO轉換部份函式化
            //return result.ToList().Select(a => ItemToDto(a));
            return Ok(result.ToList().Select(a => ItemToDto(a)));
        }

        // GET api/Todo/1f3012b6-71ae-4e74-88fd-018ed53ed2d3
        //https://localhost:7232/api/todo/450e22de-f9c1-44e2-948b-2f8f734118cb
        [HttpGet("{id}")]
        //public TodoListDto Get(Guid id)
        public ActionResult<TodoListDto> Get(Guid id)
        {
            // 沒有做關聯的寫法
            //var result = (from a in _todoContext.TodoList
            //              where a.TodoId == id
            //              select new TodoListDto
            //              {
            //                  Enable = a.Enable,
            //                  InsertTime = a.InsertTime,
            //                  Name = a.Name,
            //                  Orders = a.Orders,
            //                  TodoId = a.TodoId,
            //                  UpdateTime = a.UpdateTime,
            //                  InsertEmployeeName = a.InsertEmployee.Name,
            //                  UpdateEmployeeName = a.UpdateEmployee.Name,
            //                  UploadFiles = (from b in _todoContext.UploadFile
            //                                 where a.TodoId == b.TodoId
            //                                 select new UploadFileDto
            //                                 {
            //                                     Name = b.Name,
            //                                     Src = b.Src,
            //                                     TodoId = b.TodoId,
            //                                     UploadFileId = b.UploadFileId
            //                                 }).ToList()
            //              }
            //                ).SingleOrDefault();



            // 有做關聯的寫法
            var result = (from a in _todoContext.TodoList
                          where a.TodoId == id
                          select a)
                          .Include(a => a.InsertEmployee)
                          .Include(a => a.UpdateEmployee)
                          .Include(a => a.UploadFiles)
                          .SingleOrDefault();
            if (result == null)
            {
                return NotFound("找不到Id:" +id+ "的資料");
            }

            //return Ok(ItemToDto(result)); // 函式化
            return ItemToDto(result); // 函式化
            //return Ok(result);


        }

        [HttpGet("AutoMapper")]
        public IEnumerable<TodoListDto> GetAutoMapper([FromQuery] TodoSelectParameter value)
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

            var map = _mapper.Map<IEnumerable<TodoListDto>>(result);

            return map;
            //return null;
        }

        [HttpGet("AutoMapper/{id}")]
        public TodoListDto GetAutoMapper(Guid id)
        {
            var result = (from a in _todoContext.TodoList
                          where a.TodoId == id
                          select a)
                          .Include(a => a.UpdateEmployee)
                          .Include(a => a.InsertEmployee)
                          .Include(a => a.UploadFiles)
                          .SingleOrDefault();

            return _mapper.Map<TodoListDto>(result);
            //return null;
        }

        [HttpGet("From/{id}")]
        public dynamic GetFrom(string id, string id2, string id3, string id4)
        {
            List<dynamic> result = new List<dynamic>();

            result.Add(id);
            result.Add(id2);
            result.Add(id3);
            result.Add(id4);

            return result;
        }

        [HttpGet("GetSQL")]
        public IEnumerable<TodoList> GetSQL(string name)
        {
            string sql = "select * from todolist where 1=1";

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = sql + " and name like N'%" + name + "%'";
            }

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
      ,b.Name as InsertEmployeeName
      ,c.Name as UpdateEmployeeName
  FROM [TodoList] a
  join Employee b on a.InsertEmployeeId=b.EmployeeId
  join Employee c on a.UpdateEmployeeId=c.EmployeeId where 1=1";

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = sql + " and name like N'%" + name + "%'";
            }

            //var result = _todoContext.ExecSQL<TodoListDto>(sql);

            //return result;
            return null;
        }


        // POST api/<TodoController>
        [HttpPost]
        public void Post([FromBody] TodoListPostDto value)
        {
            //if (!ModelState.IsValid)
            //{
            //    //return "no";
            //    return Json($"Phone {phone} has an invalid format. Format: ###-###-####");  
            //}

            // 人工對應
            //List<UploadFile> up1 = new List<UploadFile>();
            //foreach (var temp in value.UploadFiles)
            //{
            //    UploadFile up = new UploadFile
            //    {
            //        Name = temp.Name,
            //        Src = temp.Src
            //    };
            //    up1.Add(up);
            //}

            TodoList insert = new TodoList
            {
                //Name = value.Name,
                //Enable = value.Enable,
                //Orders = value.Orders,
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("8840a700-35a4-4301-93aa-f172a28a7583"),
                UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024"),
                //UploadFiles = value.UploadFiles // 同時加上子資料
                //UploadFiles = up1 // 同時加上子資料
            };

            _todoContext.TodoList.Add(insert).CurrentValues.SetValues(value);
            _todoContext.SaveChanges();

            foreach (var temp in value.UploadFiles)
            {
                _todoContext.UploadFile.Add(new UploadFile()
                {
                    //Name = temp.Name,
                    //Src = temp.Src,
                    TodoId = insert.TodoId // 拿到父親的TodoId
                }).CurrentValues.SetValues(temp);
                //_todoContext.UploadFile.Add(temp);
            }
            _todoContext.SaveChanges();
        }

        // POST api/<TodoController>
        [HttpPost("nofk")]
        public void Postnofk([FromBody] TodoListPostDto value)
        {
            //新增父親後
            TodoList insert = new TodoList
            {
                Name = value.Name,
                Enable = value.Enable,
                Orders = value.Orders,
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("8840a700-35a4-4301-93aa-f172a28a7583"),
                UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024"),
                //UploadFiles = value.UploadFiles // 同時加上子資料
            };
            _todoContext.TodoList.Add(insert);
            _todoContext.SaveChanges(); // 要先insert動作

            //再來新增兒子
            foreach(var temp in value.UploadFiles)
            {
                UploadFile insert2 = new UploadFile
                {
                    Name = temp.Name,
                    Src = temp.Src,
                    TodoId = insert.TodoId // 拿到父親的TodoId
                };
                _todoContext.UploadFile.Add(insert2);
            }

            _todoContext.SaveChanges();

        }

        //-----------autpmapper 新增
        [HttpPost("AutoMapper")]
        public void PostAutoMapper([FromBody] TodoListPostDto value)
        {
            var map = _mapper.Map<TodoList>(value); // 很多轉成一行
            //手動給值
            map.InsertTime = DateTime.Now;
            map.UpdateTime = DateTime.Now;
            map.InsertEmployeeId = Guid.Parse("8840a700-35a4-4301-93aa-f172a28a7583");
            map.UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024");

            _todoContext.TodoList.Add(map);
            _todoContext.SaveChanges();
        }

        //-----------使用SQL 新增
        [HttpPost("postSQL")]
        public void PostSQL([FromBody] TodoListPostDto value)
        {
            // 避免Sql injection
            var name = new SqlParameter("name", value.Name);

            string sql = @"INSERT INTO [dbo].[TodoList]
           ([Name]
           ,[InsertTime]
           ,[UpdateTime]
           ,[Enable]
           ,[Orders]
           ,[InsertEmployeeId]
           ,[UpdateEmployeeId])
     VALUES
            (@name,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"+value.Enable+"','"+value.Orders+ "','8840a700-35a4-4301-93aa-f172a28a7583','63F8FD9D-E045-4C78-A491-96EABE1D2024')";
            //(N'" + value.Name + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"+value.Enable+"','"+value.Orders+ "','8840a700-35a4-4301-93aa-f172a28a7583','63F8FD9D-E045-4C78-A491-96EABE1D2024')";

            _todoContext.Database.ExecuteSqlRaw(sql, name);
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
                InsertEmployeeName = a.InsertEmployee.Name +"("+a.InsertEmployeeId + ")",
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
