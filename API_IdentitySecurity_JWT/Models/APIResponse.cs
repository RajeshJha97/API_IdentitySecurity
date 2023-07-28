using System.Net;

namespace API_IdentitySecurity_JWT.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}
