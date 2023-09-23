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

        /// <summary>
        /// Get a user's profile
        /// </summary>
        /// <returns>A response with the user's details</returns>
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


        /// <summary>
        /// Update a user's bank details
        /// </summary>
        /// <param name="userbank">The request body with the bank details</param>
        /// <returns>nothing</returns>
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


        /// <summary>
        /// Get all user for an organization
        /// </summary>
        /// <returns>A response containing all the users in an organization</returns>
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

        /// <summary>
        /// Search for a user by their email
        /// </summary>
        /// <param name="email">The email of the user to be retrieved</param>
        /// <returns>The user that owns the specified email</returns>
        /// <response code="200">Returns the user</response>
        [HttpGet("search/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var response = await _userService.SearchForUser(email);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
