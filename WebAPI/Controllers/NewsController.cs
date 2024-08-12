using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Dtos;
using WebAPI.Models;
using WebAPI.Parameters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly WebContext _webContext;
        public NewsController(WebContext webContext)
        {
            _webContext = webContext;
        }

        // GET: api/<NewsController>
        [HttpGet]
        //public IEnumerable<News> Get()
        // api/news?Title=xxxx&Content=xxxx&StartDateTime=xxxx
        public IEnumerable<NewsDto> Get([FromQuery] NewsParameter value)
        {
            var result = from a in _webContext.News
                         select new NewsDto
                         {
                             NewsId = a.NewsId,
                             Title = a.Title,
                             Content = a.Content,
                             StartDateTime = a.StartDateTime,
                             EndDateTime = a.EndDateTime,
                             Click = a.Click,
                             Orders = a.Orders
                         }; // .ToList() 這樣就會去撈資料庫
            
            // 這裡還沒有真正去資料庫撈資料
            // 關鍵字搜尋
            // https://localhost:7232/api/News?title=第二個新聞
            if (!string.IsNullOrWhiteSpace(value.title))
            {
                result = result.Where(x => x.Title == value.title);
            }
            // https://localhost:7232/api/News?content=天氣
            if (!string.IsNullOrWhiteSpace(value.content))
            {
                result = result.Where(x => x.Content.Contains(value.content));
            }
            //https://localhost:7232/api/News?content=天氣&startDateTime=2024-07-18
            if (value.startDateTime != null)
            {
                // (DateTime) 強制轉型
                result = result.Where(x => x.StartDateTime.Date == ((DateTime)value.startDateTime).Date);
            }

            //https://localhost:7232/api/News?minOrder=3&maxOrder=4
            //https://localhost:7232/api/News?Order=3-4
            if (value.minOrder != null && value.maxOrder != null)
            {
                result = result.Where(a => a.Orders >= value.minOrder && a.Orders <= value.maxOrder);
            }

            //最終才會去資料庫撈資料
            return result;
            //return _webContext.News;
            //return new string[] { "value1", "value2" };
        }

        //[HttpGet("one")]
        //public string Get(int id)
        //{
        //    return "value2";
        //}

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public NewsDto GetOne(Guid id)
        {
            //var result = _webContext.News.Find(id);
            var result = (from a in _webContext.News
                          where a.NewsId == id
                          select new NewsDto
                          {
                              NewsId = a.NewsId,
                              Title = a.Title,
                              Content = a.Content,
                              StartDateTime = a.StartDateTime,
                              EndDateTime = a.EndDateTime,
                              Click = a.Click
                          }).SingleOrDefault(); // 只會撈出一筆的意思

            return result;
        }

        // POST api/<NewsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("From/{id}")] //  有變數，是吃route, 要用FromRoute
        //[HttpGet("From/aa")] //  沒有變數，是吃Query, 要用FromQuery
        //https://localhost:7232/api/News/From/id?id2=id2 
        // id從route來，id2從 query來
        // id3從 form來
        //public dynamic GetFrom(string id,string id2, //[FromBody] string id3,
        //    string id3,string id4)
        public dynamic GetFrom(
            [FromRoute] string id,
            [FromQuery] string id2,
            //[FromBody] string id3,
            [FromForm] string id3,
            [FromForm] string id4
            )
        {
            List<dynamic> result = new List<dynamic>();

            result.Add(id);
            result.Add(id2);
            result.Add(id3);
            result.Add(id4);

            return result;
        }
    }
}
