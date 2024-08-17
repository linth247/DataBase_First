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
using System.Text.Json;
using WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
//using AutoMapper;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly WebContext _todoContext;
        private readonly TodoListService _todoListService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public TodoController(
            WebContext todoContext, 
            IMapper mapper,
            TodoListService todoListService,
            IWebHostEnvironment env)
        //public TodoController(WebContext todoContext)
        {
            _todoContext = todoContext;
            _mapper = mapper;
            _todoListService = todoListService;
            _env = env;
        }

        // GET: api/<TodoController>
        [HttpGet]
        //[Authorize]
        //public IEnumerable<TodoListDto> Get([FromQuery] TodoSelectParameter value)
        public IActionResult Get([FromQuery] TodoSelectParameter value)
        {
            // 控制邏輯，控制資料流向
            var result = _todoListService.取得資料(value);

            // 控制有沒有資料
            if (result == null || result.Count() <= 0)
            {
                return NotFound("找不到資源");
            }

            return Ok(result);


            //var result = _todoContext.TodoList
            //    .Include(a => a.InsertEmployee)
            //    .Include(a => a.UpdateEmployee)
            //    .Include(a => a.UploadFiles)
            //    .Select(a => a);

            //if (!string.IsNullOrWhiteSpace(value.name))
            //{
            //    result = result.Where(a => a.Name.IndexOf(value.name) > -1);
            //}

            //if (value.enable != null)
            //{
            //    result = result.Where(a => a.Enable == value.enable);
            //}

            //if (value.InsertTime != null)
            //{
            //    result = result.Where(a => a.InsertTime.Date == value.InsertTime);
            //}

            //if (value.minOrder != null && value.maxOrder != null)
            //{
            //    result = result.Where(a => a.Orders >= value.minOrder && a.Orders <= value.maxOrder);
            //}

            //if (result == null || result.Count() <=0)
            //{
            //    return NotFound("找不到資源");
            //}

            //// 將DTO轉換部份函式化
            ////return result.ToList().Select(a => ItemToDto(a));
            //return Ok(result.ToList().Select(a => ItemToDto(a)));
        }

        // GET api/Todo/1f3012b6-71ae-4e74-88fd-018ed53ed2d3
        //https://localhost:7232/api/todo/450e22de-f9c1-44e2-948b-2f8f734118cb
        [HttpGet("{TodoId}")]
        //public TodoListDto Get(Guid id)
        public ActionResult<TodoListDto> GetOne(Guid TodoId)
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
            //var result = (from a in _todoContext.TodoList
            //              where a.TodoId == TodoId
            //              select a)
            //              .Include(a => a.InsertEmployee)
            //              .Include(a => a.UpdateEmployee)
            //              .Include(a => a.UploadFiles)
            //              .SingleOrDefault();

            var result = _todoListService.取得單筆資料(TodoId);
            if (result == null)
            {
                return NotFound("找不到Id:" + TodoId + "的資料");
            }

            //return Ok(ItemToDto(result)); // 函式化
            //return ItemToDto(result); // 函式化
            return result;
            //return Ok(result);


        }

        [HttpGet("AutoMapper")]
        public IEnumerable<TodoListDto> GetAutoMapper([FromQuery] TodoSelectParameter value)
        {
            //var result = _todoContext.TodoList
            //    .Include(a => a.InsertEmployee)
            //    .Include(a => a.UpdateEmployee)
            //    .Include(a => a.UploadFiles)
            //    .Select(a => a);

            //if (!string.IsNullOrWhiteSpace(value.name))
            //{
            //    result = result.Where(a => a.Name.IndexOf(value.name) > -1);
            //}

            //if (value.enable != null)
            //{
            //    result = result.Where(a => a.Enable == value.enable);
            //}

            //if (value.InsertTime != null)
            //{
            //    result = result.Where(a => a.InsertTime.Date == value.InsertTime);
            //}

            //if (value.minOrder != null && value.maxOrder != null)
            //{
            //    result = result.Where(a => a.Orders >= value.minOrder && a.Orders <= value.maxOrder);
            //}

            //var map = _mapper.Map<IEnumerable<TodoListDto>>(result);

            //return map;
            return _todoListService.使用AutoMapper取得資料(value);
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
        public IActionResult Post([FromBody] TodoListPostDto value)
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

            // 新增資料
            //TodoList insert = new TodoList
            //{
            //    //Name = value.Name,
            //    //Enable = value.Enable,
            //    //Orders = value.Orders,
            //    InsertTime = DateTime.Now,
            //    UpdateTime = DateTime.Now,
            //    InsertEmployeeId = Guid.Parse("8840a700-35a4-4301-93aa-f172a28a7583"),
            //    UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024"),
            //    //UploadFiles = value.UploadFiles // 同時加上子資料
            //    //UploadFiles = up1 // 同時加上子資料
            //};

            //_todoContext.TodoList.Add(insert).CurrentValues.SetValues(value);
            //_todoContext.SaveChanges();

            //if(value.UploadFiles != null)
            //{
            //    foreach (var temp in value.UploadFiles)
            //    {
            //        _todoContext.UploadFile.Add(new UploadFile()
            //        {
            //            //Name = temp.Name,
            //            //Src = temp.Src,
            //            TodoId = insert.TodoId // 拿到父親的TodoId
            //        }).CurrentValues.SetValues(temp);
            //        //_todoContext.UploadFile.Add(temp);
            //    }
            //    _todoContext.SaveChanges();
            //}

            var insert = _todoListService.新增資料(value);

            return CreatedAtAction(nameof(GetOne), new { TodoId = insert.TodoId }, insert);
        }

        //58.【10.上傳檔案API】ASP.NET Core Web API 入門教學(10_3) - 使用ModelBinder處理FormData的Json字串並反序列化成對應類別物件
        [HttpPost("up")]
        //public void PostUp([FromForm] string value, [FromForm] IFormFileCollection files)
        //{
        //    TodoList aa = JsonSerializer.Deserialize<TodoList>(value);
        //}
        public void PostUp([FromForm] TodoListPostUpDto value)
        {
            // 新增一筆資料
            TodoList insert = new TodoList
            {
                InsertTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                InsertEmployeeId = Guid.Parse("8840a700-35a4-4301-93aa-f172a28a7583"),
                UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024"),
            };

            _todoContext.TodoList.Add(insert).CurrentValues.SetValues(value.TodoList);
            _todoContext.SaveChanges();

            // 夾帶檔案
            var rootPath = _env.ContentRootPath + @"\wwwroot\UploadFiles\" + insert.TodoId + "\\";

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            foreach (var file in value.files)
            {
                if (file.Length > 0)
                {
                    var filePath = file.FileName;
                    using (var stream = System.IO.File.Create(rootPath + filePath))
                    {
                        file.CopyTo(stream);

                        var insert2 = new UploadFile
                        {
                            Name = filePath,
                            Src = "/UploadFiles/" + insert.TodoId + "/" + filePath,
                            TodoId = insert.TodoId
                        };

                        _todoContext.UploadFile.Add(insert2);
                    }
                }
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
            //var map = _mapper.Map<TodoList>(value); // 很多轉成一行
            ////手動給值, 轉換程式碼
            //map.InsertTime = DateTime.Now;
            //map.UpdateTime = DateTime.Now;
            //map.InsertEmployeeId = Guid.Parse("8840a700-35a4-4301-93aa-f172a28a7583");
            //map.UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024");

            //_todoContext.TodoList.Add(map);
            //_todoContext.SaveChanges();
            _todoListService.使用AutoMapper新增資料(value);
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






        //35.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_1) - 使用PUT更新資料
        // PUT api/<TodoController>/5
        //40.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_6) - 更新資料後返回符合規範的內容
        //回傳 IActionResult
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] TodoListPutDto value)
        {
            if(id != value.TodoId)
            {
                return BadRequest();
            }
            ////_todoContext.Entry(value).State = EntityState.Modified;
            //_todoContext.TodoList.Update(value);
            //_todoContext.SaveChanges();

            //var update = _todoContext.TodoList.Find(id);
            //var update = (from a in _todoContext.TodoList 
            //              where a.TodoId == id
            //              select a).SingleOrDefault();
            //if(update != null)
            //{
            //    update.UpdateTime = DateTime.Now;
            //    update.UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024");

            //    //update.Name = value.Name;
            //    //update.Orders = value.Orders;
            //    //update.Enable = value.Enable;

            //    //38.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_4) - 使用內建函式匹配更新資料	
            //    //不用autoMapper
            //    _todoContext.TodoList.Update(update).CurrentValues.SetValues(value);

            //    _todoContext.SaveChanges();
            //}
            //else
            //{
            //    return NotFound();
            //}

            if(_todoListService.修改資料(id, value) == 0)
            {
                return NotFound();
            }

            return NoContent(); // 204 伺服器成功的處理了請求，沒有返回任何內容。
            // return NotFound();
            // return NoContent();  //204
            // return Ok();  // 200
        }

        //36.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_2) - 使用DTO更新資和架構思考
        [HttpPut]
        public void Put([FromBody] TodoListPutDto value)
        {
            ////_todoContext.Entry(value).State = EntityState.Modified;
            //_todoContext.TodoList.Update(value);
            //_todoContext.SaveChanges();

            //var update = _todoContext.TodoList.Find(id);
            var update = (from a in _todoContext.TodoList
                          where a.TodoId == value.TodoId
                          select a).SingleOrDefault();
            if (update != null)
            {
                update.UpdateTime = DateTime.Now;
                 update.UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024");

                update.Name = value.Name;
                update.Orders = value.Orders;
                update.Enable = value.Enable;

                _todoContext.SaveChanges();
            }
        }

        //37.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_3) - 使用AutoMapper更新資料
        [HttpPut("AutoMapper/{id}")]
        public void PutAutoMapper(Guid id, [FromBody] TodoListPutDto value)
        {
            ////_todoContext.Entry(value).State = EntityState.Modified;
            //_todoContext.TodoList.Update(value);
            //_todoContext.SaveChanges();

            //var update = _todoContext.TodoList.Find(id);
            var update = (from a in _todoContext.TodoList
                          where a.TodoId == id
                          select a).SingleOrDefault();
            if (update != null)
            {
                //update.InsertTime = DateTime.Now;
                //update.UpdateTime = DateTime.Now;
                //update.InsertEmployeeId = Guid.Parse("8840a700-35a4-4301-93aa-f172a28a7583");
                //update.UpdateEmployeeId = Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024");

                //update.Name = value.Name;
                //update.Orders = value.Orders;
                //update.Enable = value.Enable;
                _mapper.Map(value, update); // 記得要去做對應檔的設定 TodoListProfile.cs
                // CreateMap<TodoListPutDto, TodoList>();
                _todoContext.SaveChanges();
            }
        }

        //38.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_4) - 使用內建函式匹配更新資料	
        //不用autoMapper


        //41.【7.刪除資料DELETE】ASP.NET Core Web API 入門教學(7_1) - 使用DELETE刪除資料
        // DELETE api/<TodoController>/5
        //44.【7.刪除資料DELETE】ASP.NET Core Web API 入門教學(7_4) - 刪除資料後返回符合規範的內容
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            //var delete = (from a in _todoContext.TodoList
            //              where a.TodoId == id
            //              select a).Include(c=>c.UploadFiles).SingleOrDefault();

            //if(delete == null)
            //{
            //    return NotFound("找不到刪除的資源");
            //}
            //_todoContext.TodoList.Remove(delete);
            //_todoContext.SaveChanges();

            if(_todoListService.刪除資料(id) == 0)
            {
                return NotFound("找不到刪除的資源");
            }

            return NoContent();
        }

        //42.【7.刪除資料DELETE】ASP.NET Core Web API 入門教學(7_2) - 刪除全部子資料
        [HttpDelete("nofk/{id}")]
        public void NofkDelete(Guid id)
        {
            // 有設外鍵，先刪除兒子
            var child = from a in _todoContext.UploadFile
                        where a.TodoId == id
                        select a;

            _todoContext.UploadFile.RemoveRange(child);
            _todoContext.SaveChanges();

            var delete = (from a in _todoContext.TodoList
                          where a.TodoId == id
                          select a).SingleOrDefault();
            if (delete != null)
            {
                _todoContext.TodoList.Remove(delete);
                _todoContext.SaveChanges();
            }
        }

        //43.【7.刪除資料DELETE】ASP.NET Core Web API 入門教學(7_3) - 刪除多筆指定資料
        // 如何測試：先去新增父子資料 AutoMapper ，新增兩筆資料，再把以下貼上
        // https://localhost:7232/api/todo/list/["C21AA812-4B2D-4AB2-9A40-59ED6E9D07E1","8B7077C6-5495-4123-833E-6CF490E5DD6B"]

        [HttpDelete("list/{ids}")]
        // public void Delete(List<Guid> id)
        public void Delete(string ids)
        {
            //["C21AA812-4B2D-4AB2-9A40-59ED6E9D07E1","8B7077C6-5495-4123-833E-6CF490E5DD6B"]
            List<Guid> deleteList = JsonSerializer.Deserialize<List<Guid>>(ids);

            var delete = (from a in _todoContext.TodoList
                          where deleteList.Contains(a.TodoId)
                          select a).Include(c => c.UploadFiles);

            _todoContext.TodoList.RemoveRange(delete);
            _todoContext.SaveChanges();
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
