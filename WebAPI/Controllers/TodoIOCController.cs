using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Parameters;
using WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoIOCController : ControllerBase
    {
        private readonly ITodoListService _todoListService;

        public TodoIOCController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        // GET: api/<TodoIOCController>
        [HttpGet]
        public List<TodoListDto> Get([FromQuery]TodoSelectParameter value)
        {
            return _todoListService.取得資料(value);
        }

    }
}
