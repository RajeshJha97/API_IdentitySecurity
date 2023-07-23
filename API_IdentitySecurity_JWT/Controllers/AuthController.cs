using API_IdentitySecurity_JWT.DTO;
using API_IdentitySecurity_JWT.Models;
using API_IdentitySecurity_JWT.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;
using System.Security.Claims;

namespace API_IdentitySecurity_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private string secretKey;
        private readonly IMapper _mapper;
        private APIResponse _resp;
        private Tokens _token;
        public AuthController(IConfiguration config,IMapper mapper)
        {
            secretKey = config.GetValue<string>("SecretKey");
            _mapper = mapper;
            _resp = new();
            _token = new();
        }

        [HttpGet("TestSecretKey")]
        public ActionResult<APIResponse> TestSecretKey()
        {
            _resp.StatusCode=HttpStatusCode.OK;
            _resp.IsSuccess=true;
            _resp.Result = $"{secretKey}, count: {secretKey.Length}";
            return Ok(_resp);
        }

        [HttpPost("User Registration")]
        public ActionResult<APIResponse> UserRegister([FromBody] RegisterDTO userRegister)
        {
            _resp.StatusCode=HttpStatusCode.OK;
            _resp.IsSuccess=true;
            _resp.Result = $"{userRegister.Username} as been registered successfully {DateTime.Now}";
            return Ok(_resp);
        }

        [HttpPost("SignIn")]
        public ActionResult<APIResponse> SignIn([FromBody] SignIn user)
        {
            try
            {
                if (user.Username.ToLower() == "admin" && user.Password == "password")
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Email,"admin@admin.com"),
                        new Claim("admin","true")

                    };
                    var expires_At = DateTime.Now.AddMinutes(10);
                    _resp.StatusCode = HttpStatusCode.OK;
                    _resp.IsSuccess=true;
                    _resp.Result = new {
                        token = _token.GenerateTokens(claims, expires_At, secretKey),
                        expiresAt=expires_At
                    
                    };
                    return Ok(_resp);
                }
                ModelState.AddModelError("Unauthorized", "Invalid Credentials");
                _resp.StatusCode = HttpStatusCode.Unauthorized;
                _resp.Result = ModelState.Values;
                return Unauthorized(_resp);
            }
            catch(Exception ex) 
            {
                _resp.StatusCode = HttpStatusCode.BadRequest;
                _resp.ErrorMessage = ex.Message;
                return Ok(_resp);
            }
        }

        
    }
}
