using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService;

        public AuthController(ILogger<AuthController> logger, IUserRepository userService)
        {
            _userService = userService;
        }


        [HttpPost("user/signup")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDTO model)
        {
            var res = await _userService.CreateUser(model);
            return Ok(res);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO model)
        {
            var res = await _userService.Login(model);
            return Ok(res);
        }
    }
}