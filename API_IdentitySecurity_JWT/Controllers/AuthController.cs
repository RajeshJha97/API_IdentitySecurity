using API_IdentitySecurity_JWT.DTO;
using API_IdentitySecurity_JWT.Models;
using API_IdentitySecurity_JWT.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        //private ILogger _logger;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;


        public AuthController(IConfiguration config,IMapper mapper, /*ILogger logger,*/UserManager<IdentityUser>userManager, SignInManager<IdentityUser> signInManager)
        {
            //_logger = logger;
            secretKey = config.GetValue<string>("SecretKey");
            _mapper = mapper;
            _resp = new();
            _token = new();
            _userManager = userManager;
            _signInManager = signInManager;
        }

       
        [HttpPost("UserRegistration")]
        public async Task<ActionResult<APIResponse>> UserRegister([FromBody] RegistrationDTO userRegister)
        {
            if (!ModelState.IsValid)
            {
                _resp.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_resp);
            }
            if (userRegister.Password != userRegister.ConfirmPassword)
            {
                _resp.StatusCode = HttpStatusCode.BadRequest;
                _resp.ErrorMessage = "Password and Confirm Password must be the same";
                return BadRequest(_resp);
            }
            var user = new IdentityUser
            {
                Email= userRegister.Email,
                UserName=userRegister.Email,

            };
            var result=await _userManager.CreateAsync(user, userRegister.Password);
            if (result.Succeeded)
            {
                _resp.StatusCode = HttpStatusCode.Created;
                _resp.Result = $"{userRegister.Email} has been created successfully";
                return Ok(_resp);
            }
            else 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegistrationError", error.Description);
                }
                _resp.StatusCode = HttpStatusCode.BadRequest;
                _resp.ErrorMessage = ModelState;
                return BadRequest(_resp);
            }
        }

        [HttpPost("SignIn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> SignIn([FromBody] SignIn user)
        {
            try
            {
                var result=await _signInManager.PasswordSignInAsync(user.Username, user.Password,false,false);
                if (result.Succeeded) 
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Email,"admin@admin.com"),
                        new Claim("admin","true"),
                        new Claim("manager","true")

                    };
                    var expires_At = DateTime.Now.AddMinutes(10);
                    _resp.StatusCode = HttpStatusCode.OK;
                    _resp.IsSuccess = true;
                    _resp.Result = new
                    {
                        token = _token.GenerateTokens(claims, expires_At, secretKey),
                        expiresAt = expires_At

                    };
                    return Ok(_resp);
                }

                //if (user.Username.ToLower() == "admin" && user.Password == "password")
                //{
                //    List<Claim> claims = new List<Claim>()
                //    {
                //        new Claim(ClaimTypes.Name,user.Username),
                //        new Claim(ClaimTypes.Email,"admin@admin.com"),
                //        new Claim("admin","true")

                //    };
                //    var expires_At = DateTime.Now.AddMinutes(10);
                //    _resp.StatusCode = HttpStatusCode.OK;
                //    _resp.IsSuccess=true;
                //    _resp.Result = new {
                //        token = _token.GenerateTokens(claims, expires_At, secretKey),
                //        expiresAt=expires_At

                //    };
                //    return Ok(_resp);
                //}
                ModelState.AddModelError("Unauthorized", "Invalid Credentials");
                _resp.StatusCode = HttpStatusCode.Unauthorized;
                _resp.Result = ModelState;
                return Unauthorized(_resp);
            }
            catch(Exception ex) 
            {
                _resp.StatusCode = HttpStatusCode.BadRequest;
                _resp.ErrorMessage = ex.Message;
                return Ok(_resp);
            }
        }


        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new {Signout="User Signout successfully"});
        }
        
    }
}
