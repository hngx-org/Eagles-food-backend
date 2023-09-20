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
            _userService = userService;
        }


        [HttpPost("register")]
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