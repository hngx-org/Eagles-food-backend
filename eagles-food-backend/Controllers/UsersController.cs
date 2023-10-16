using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Filters;
using eagles_food_backend.Services.UserServices;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// Get The organization a user belongs to
        /// </summary>
        /// <returns>A response of user's Organization details</returns>
        [HttpGet("organization")]
        public async Task<IActionResult> GetUserOrg()
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var userProfile = await _userService.GetUserOrg(id);
                return StatusCode((int)userProfile.statusCode, userProfile);
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
        /// Updates the user's details
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The updated user details</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDTO model)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var userprofile = await _userService.UpdateUserProfile(id, model);
                return StatusCode((int)userprofile.statusCode, userprofile);
            }
            return BadRequest();
        }

        [HttpPost("photo")]
        public async Task<IActionResult> UpdatePhoto([Required] IFormFile file)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var userprofile = await _userService.UploadPhoto(file, id);
                return StatusCode((int)userprofile.statusCode, userprofile);
            }
            return BadRequest();

        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>A response containing all the users divided by whether they're in the callers org. or not</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetUsersForOrganization([FromQuery] PaginationFilter filter)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var response = await _userService.GetAllUsersByOrganization(id, validFilter);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }

        [HttpGet("others")]
        public async Task<IActionResult> GetUsersForOtherOrganizations([FromQuery] PaginationFilter filter)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var response = await _userService.GetAllUsersOutsideOrganization(id, validFilter);
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
        
        /// <summary>
        /// Search for a user by their name
        /// </summary>
        /// <param name="email">The email of the user to be retrieved</param>
        /// <returns>The user that owns the specified email</returns>
        /// <response code="200">Returns the user</response>
        [HttpGet("searchname/{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var response = await _userService.SearchForUserByName(name);
            return StatusCode((int)response.statusCode, response);
        }

        /// <summary>
        /// This end point is for users to see all thier pending invites
        /// </summary>
        /// <returns>It returns all the invites a person has unattended to </returns>
        /// <response code="200">Returns the user</response>
        [HttpGet("userinvites")]
        public async Task<IActionResult> GetUserInvites()
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var response = await _userService.UserInvites(id);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }

        /// <summary>
        /// This end point is for users accept invites
        /// send true for yes(acceptance) and false for no(rejection)
        /// </summary>
        /// <returns>It returns all the invites a person has unattended to </returns>
        /// <response code="200">Returns the user</response>
        [HttpPost("toggleinvite")]
        public async Task<IActionResult> Invite([FromBody] ToggleInviteDTO model)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var response = await _userService.ToggleInvite(id, model);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }

        /// <summary>
        /// This end point is for users accept invites
        /// send true for yes(acceptance) and false for no(rejection)
        /// </summary>
        /// <returns>It returns all the invites a person has unattended to </returns>
        /// <response code="200">Returns the user</response>
        [HttpPost("requesttojoinOrg/{orgId}")]
        public async Task<IActionResult> SendOrganizationInviteRequest(int orgId)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var response = await _userService.SendInvitationRequest(id, orgId);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }
    }
}