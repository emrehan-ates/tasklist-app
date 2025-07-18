using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;
using TaskApi.Services;
using System.IdentityModel.Tokens.Jwt;
using TaskApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TaskApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {


        private readonly IUserService _service;
        private readonly IConfiguration _config;
        private readonly TokenGenerate _tokenGenerator;
        private readonly ILogService _logger;
        private readonly IDatabase _redis;
        public UserController(IUserService service, IConfiguration config, TokenGenerate tokenGenerator, ILogService logger, IConnectionMultiplexer redis)
        {
            _service = service;
            _config = config;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
            _redis = redis.GetDatabase();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDTO newuser)
        {
            var existing = await _service.GetUserByEmail(newuser.user_email);
            if (existing != null)
            {
                _logger.LogWarning("User already existing error", new { user_email = newuser.user_email });
                return BadRequest("Email is already registered!");
            }

            newuser.user_password = BCrypt.Net.BCrypt.HashPassword(newuser.user_password);

            await _service.AddUser(newuser);

            _logger.LogInfo("User Registered", new { user_email = newuser.user_email });
            return Ok(new { msg = "User registered!!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO info)
        {
            var user = await _service.ValidateUser(info.user_email, info.user_password);
            if (user == null)
            {
                _logger.LogWarning("Invalid Credentials", new { user_email = info.user_email });
                return Unauthorized("Invalid email or password!!!");

            }

            var token = _tokenGenerator.TokenGenerator(user);

            var message = "User logged in";
            var cleanMessage = message?.Replace("\0", "") ?? "";
            _logger.LogInfo(cleanMessage, new { user_id = user.user_id });
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete()
        {
            var user_id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            await _service.DeleteUser(int.Parse(user_id));

            _logger.LogInfo("User Deleted ", new { user_id = user_id });
            return Ok("Deleted you ahahahah!!");
        }

        [Authorize]
        [HttpGet]
        [Route("getprofile")]
        public async Task<IActionResult> GetProfile()
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim type: {claim.Type}, value: {claim.Value}");
            }
            // var subClaim = User.FindFirst(JwtRegisteredClaimNames.NameId);
            // if (subClaim == null)
            //     return Unauthorized("Token'da 'sub' claim bulunamadı.");
            var user_id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var profile = await _service.GetProfile(int.Parse(user_id));
            _logger.LogInfo("Profile reached", new { user_id = user_id });
            return Ok(profile);
        }

        // [Authorize]
        // [HttpPost]
        // [Route("logout")]
        // public async IActionResult Logout()
        // {
        //     var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        //     if (string.IsNullOrEmpty(token))
        //     {
        //         return BadRequest("No token found");

        //     }

        //     var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        // }



    }

}