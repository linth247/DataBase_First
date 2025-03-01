﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/Todo/{TodoId}/UploadFile")]
    [ApiController]
    public class TodoUploadFileController : ControllerBase
    {
        private readonly WebContext _todoContext;
        private readonly IMapper _mapper;

        public TodoUploadFileController(WebContext todoContext, IMapper mapper)
        //public TodoController(WebContext todoContext)
        {
            _todoContext = todoContext;
            _mapper = mapper;
        }

        // GET: api/<TodoUploadFileController>
        //"api/Todo/{TodoId}/UploadFile")
        [HttpGet]
        public ActionResult<IEnumerable<UploadFileDto>> Get(Guid TodoId)
        {
            if(!_todoContext.TodoList.Any(a=>a.TodoId==TodoId))
            {
                return NotFound("找不到該事項");
            }

            var result = from a in _todoContext.UploadFile
                         where a.TodoId == TodoId
                         select new UploadFileDto
                         {
                             Name = a.Name,
                             Src = a.Src,
                             TodoId = a.TodoId,
                             UploadFileId = a.UploadFileId
                         };

            if(result == null || result.Count() == 0)
            {
                return NotFound("找不到檔案");
            }

            return Ok(result);
        }

        // GET api/<TodoUploadFileController>/5
        //"api/Todo/{TodoId}/UploadFile/{UploadFileId}"
        [HttpGet("{UploadFileId}")]
        public ActionResult<UploadFileDto> Get(Guid TodoId, Guid UploadFileId)
        {
            if (!_todoContext.TodoList.Any(a => a.TodoId == TodoId))
            {
                return NotFound("找不到該事項");
            }
            var result = (from a in _todoContext.UploadFile
                         where a.TodoId == TodoId
                         && a.UploadFileId == UploadFileId
                          select new UploadFileDto
                         {
                             Name = a.Name,
                             Src = a.Src,
                             TodoId = a.TodoId,
                             UploadFileId = a.UploadFileId
                         }).SingleOrDefault();

            if(result == null)
            {
                return NotFound("找不到檔案");
            }

            return result;
        }

        // POST api/<TodoUploadFileController>
        [HttpPost]
        public string Post(Guid TodoId, [FromBody] UploadFilePostDto value)
        {
            if (!_todoContext.TodoList.Any(a => a.TodoId == TodoId))
            {
                return "找不到該事項";
            }

            UploadFile insert = new UploadFile
            {
                Name = value.Name,
                Src = value.Src,
                TodoId = TodoId
            };

            _todoContext.UploadFile.Add(insert);
            _todoContext.SaveChanges();

            return "ok";
        }

        //-----------autoMapper 新增一筆uploadfile
        [HttpPost("AutoMapper")]
        public string PostAutoMapper(Guid TodoId, [FromBody] UploadFilePostDto value)
        {
            if (!_todoContext.TodoList.Any(a => a.TodoId == TodoId))
            {
                return "找不到該事項";
            }

            var map = _mapper.Map<UploadFile>(value);
            map.TodoId = TodoId;

            _todoContext.UploadFile.Add(map);
            _todoContext.SaveChanges();

            return "ok";
        }

        // PUT api/<TodoUploadFileController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodoUploadFileController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
