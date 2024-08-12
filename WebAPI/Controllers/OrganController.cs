using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganController : ControllerBase
    {
        private readonly WebContext _webContext;
        public OrganController(WebContext webContext)
        {
            _webContext = webContext;
        }


        // 建立一萬筆資料
        [HttpGet("a1")]
        public void a1()
        {
            for (int i = 10000; i < 20000; i++)
            {
                var insert = new Organ
                {
                    FatherOrganId = Guid.Parse("e38865ff-15bb-408b-b130-01bbf1f82881"),
                    Name = "局" + i,
                    Src = "http"
                };
                _webContext.Add(insert);
            }
            _webContext.SaveChanges();
        }

        [HttpGet("GetSQL")]
        public IEnumerable<Organ> GetSQL(string name)
        {
            string sql = "select * from organ where 1=1";

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = sql + " and name like N'%" + name + "%'";
            }

            var result = _webContext.Organ.FromSqlRaw(sql);

            return result;

        }

        //[HttpGet("GetSQLDto")]
        //public IEnumerable<OrganSelectDto> GetSQLDto(string name)
        //{
        //    string sql = "select * from organ where 1=1";

        //    if(!string.IsNullOrWhiteSpace(name))
        //    {
        //        sql = sql + " and name like N'%" + name + "%'";
        //    }

        //    var result = _webContext.OrganSelectDto.FromSqlRaw(sql);

        //    return result;

        //}

        // GET: api/<OrganController>
        [HttpGet]
        public IEnumerable<OrganSelectDto> Get()
        {
            var result = (from a in _webContext.Organ
                          select new OrganSelectDto
                          {
                              FatherOrganId = a.FatherOrganId,
                              Name = a.Name,
                              OrganId = a.OrganId,
                              Src = a.Src
                          }).ToList();

            //foreach (var item in result)
            //{
            //    item.Two = (from a in _webContext.Organ
            //                where a.FatherOrganId == item.FatherOrganId
            //                select new 第二兒子
            //                {
            //                    FatherOrganId = a.FatherOrganId,
            //                    Name = a.Name,
            //                    OrganId = a.OrganId,
            //                    Src = a.Src
            //                }).ToList();

            //}
            return result;
            
            //return new string[] { "value1", "value2" };
        }


        [HttpGet("get3")]
        public IEnumerable<OrganSelectDto> Get3()
        {
            // 先把所有資料撈回來，撈回到我們的記憶體之後
            var _organs = _webContext.Organ.ToList();
            var result = (from a in _organs
                          select new OrganSelectDto
                          {
                              FatherOrganId = a.FatherOrganId,
                              Name = a.Name,
                              OrganId = a.OrganId,
                              Src = a.Src
                          }).ToList();

                    foreach (var item in result)
                    {
                        item.Two = (from a in _organs
                                    where a.FatherOrganId == item.FatherOrganId
                                    //select new 第二兒子
                                    select new 第二兒子
                                    {
                                        FatherOrganId = a.FatherOrganId,
                                        Name = a.Name,
                                        OrganId = a.OrganId,
                                        Src = a.Src
                                    }).FirstOrDefault();
                    }
                    return result;

            //return new string[] { "value1", "value2" };
        }

        [HttpGet("get5")]
        public IEnumerable<Organ> Get5() // 不會有資源秏盡的問題
        {
            var result = from a in _webContext.Organ
                          where a.OrganId == Guid.Parse("E20B8D81-799B-4170-8FBE-000506E3ED4E")
                          select a;

            return result;
        }

        [HttpGet("get6")]
        public IEnumerable<Organ> Get6() // 記憶體會變大
        {
            var _organs = _webContext.Organ.ToList();
            var result = from a in _organs
                         where a.OrganId == Guid.Parse("E20B8D81-799B-4170-8FBE-000506E3ED4E")
                         select a;

            return result;
        }

        // GET api/<OrganController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrganController>
        [HttpPost]
        public void Post([FromBody] Organ value)
        {
            _webContext.Add(value);
            _webContext.SaveChanges();
        }

        // PUT api/<OrganController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrganController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
