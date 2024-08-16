namespace WebAPI.Services
{
    public class TransientService
    {
        public int 次數 = 0;

        public void 次數加一()
        {
            次數 = 次數 + 1;
        }
    }
}
