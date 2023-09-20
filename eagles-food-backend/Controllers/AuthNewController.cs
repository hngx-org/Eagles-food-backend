using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;

        public AuthController(ILogger<AuthController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO model)
        {
            var createdUser = await _userService.CreateUser(model);
            return createdUser.Status ? Ok(createdUser) : BadRequest(createdUser);
        }
    }
}