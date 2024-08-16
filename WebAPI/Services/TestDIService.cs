namespace WebAPI.Services
{

    public class TestDIService
    {
        private readonly TransientService _transientService;
        private readonly SingletonService _singletonService;
        private readonly ScopedService _scopedService;

        public TestDIService(
            TransientService transientService,
            SingletonService singletonService,
            ScopedService scopedService)
        {
            _transientService = transientService;
            _singletonService = singletonService;
            _scopedService = scopedService;
        }

        public void 執行()
        {
            _transientService.次數加一();
            _singletonService.次數加一();
            _scopedService.次數加一();
        }
    }
}
