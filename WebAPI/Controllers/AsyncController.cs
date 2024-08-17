using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsyncController : ControllerBase
    {
        private readonly AsyncService _asyncService;
        public AsyncController(AsyncService asyncService)
        {
            _asyncService = asyncService;
        }
        // GET: api/<AsyncController>
        [HttpGet]
        public async Task<int> Get()
        {
            //Console.Write("AsyncController");
            return await _asyncService.主作業();
            //return 1;
        }


    }
}
