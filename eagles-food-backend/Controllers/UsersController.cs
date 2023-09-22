using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eagles_food_backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userService;

        public UsersController(IUserRepository userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var userprofile = await _userService.GetUserProfile(id);
                return StatusCode((int)userprofile.statusCode, userprofile);
            }
            else return BadRequest();

        }

        [HttpPut("bank")]
        public async Task<IActionResult> UpdateUserBank([FromBody] UserBankUpdateDTO userbank)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var userprofile = await _userService.UpdateUserBank(userbank, id);

                return StatusCode((int)userprofile.statusCode, userprofile);
            }
            else return BadRequest();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] CreateUserDTO model)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var userprofile = await _userService.GetUserProfile(id);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUsersForOrganization()
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                //var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                //if (role != "admin")
                //{
                //    return Unauthorized();
                //}
                var response = await _userService.GetAllUsersForOrganization(id);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }

        [HttpGet("search/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var response = await _userService.SearchForUser(email);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
