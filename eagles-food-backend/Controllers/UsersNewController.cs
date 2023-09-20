using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eagles_food_backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        // GET: api/<UsersController>
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            int id = 1;//Placeholder for getting id from jwt claims
            var userprofile = await _userService.GetUserProfile(id);
            return userprofile.Status ? Ok(userprofile) : BadRequest();
        }

        [HttpPut("bank")]
        public async Task<IActionResult> UpdateUserBank([FromBody] UserBankUpdateDTO userbank)
        {
            int id = 1;//Placeholder for getting id from jwt claims
            var userprofile = await _userService.UpdateUserBank(userbank, id);
            return userprofile.Status ? Ok(userprofile) : BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersForOrganization()
        {
            int org_id = 1;//Placeholder for getting org_id from either jwt claims or database
            var response = await _userService.GetAllUsersForOrganization(org_id);
            return response.Status ? Ok(response) : BadRequest();
        }

        [HttpGet("search/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var response = await _userService.SearchForUser(email);
            return response.Status ? Ok(response) : BadRequest();
        }
    }
}
