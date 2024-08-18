using Microsoft.Identity.Client;

namespace WebAPI.Dtos
{
    public class ReturnJson
    {
        public dynamic Data { get; set; }
        public int HttpCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
