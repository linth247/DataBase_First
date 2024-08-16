using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDIController : Controller
    {
        private readonly TransientService _transientService;
        private readonly SingletonService _singletonService;
        private readonly ScopedService _scopedService;
        private readonly TestDIService _testDIService;

        public TestDIController(
            TransientService transientService,
            SingletonService singletonService,
            ScopedService scopedService,
            TestDIService testDIService)
        {
            _transientService = transientService;
            _singletonService = singletonService;
            _scopedService = scopedService;
            _testDIService = testDIService;
        }

        [HttpGet]
        public 執行次數 Get()
        {
            _testDIService.執行();

            _transientService.次數加一();
            _singletonService.次數加一();
            _scopedService.次數加一();

            var result = new 執行次數
            {
                Transient = _transientService.次數,
                Scoped = _scopedService.次數,
                Singleton = _singletonService.次數
            };
            return result;
        }
    }
}
