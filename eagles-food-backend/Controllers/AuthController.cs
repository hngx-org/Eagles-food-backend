using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Services.UserServices;

using Microsoft.AspNetCore.Mvc;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService;

        public AuthController(IUserRepository userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/auth/user/signup
        /// {
        ///   "lastName": "john",
        ///   "firstName": "doe",
        ///   "email": "john@doe.com",
        ///   "password": "pass",
        ///   "phone": "123456"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">Request body with user's details</param>
        /// <returns>A response with 201 and the id, email of the new user and token</returns>
        /// <response code="201">Returns the id, email of the new user and token</response>
        /// <response code="400">If the email is already taken, or invalid</response>
        /// <response code="500">If there was an error creating the user</response>
        [HttpPost("user/signup")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDTO model)
        {
            var res = await _userService.CreateUser(model);
            return StatusCode((int)res.statusCode, res);
        }

        /// <summary>
        /// Login as a user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/auth/login
        /// {
        ///   "email": "john@doe.com",
        ///   "password": "pass"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">Request body with user's details</param>
        /// <returns>A response with 200 and the token</returns>
        /// <response code="200">Returns the token</response>
        /// <response code="404">If the email or password is incorrect</response>
        /// <response code="500">If there was an error logging in</response>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO model)
        {
            var res = await _userService.Login(model);
            return StatusCode((int)res.statusCode, res);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/auth/changePassword
        /// {
        ///   "email": "john@doe.com",
        ///   "oldPassword": "pass",
        ///   "newPassword": "pass",
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">Request body with user's details</param>
        /// <returns>A response with 200 and the token</returns>
        /// <response code="200">Returns the token</response>
        /// <response code="404">If the email or password is incorrect</response>
        /// <response code="500">If there was an error logging in</response>
        [HttpPost("changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var res = await _userService.ChangePassword(model);
            return StatusCode((int)res.statusCode, res);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDTO passwordDto)
        {
            var res = await _userService.ForgotUserPassword(passwordDto.Email);
            return StatusCode((int)res.statusCode, res);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO resetDto)
        {
            var res = await _userService.ResetUserPassword(resetDto);
            return StatusCode((int)res.statusCode, res);
        }

        [HttpGet("verify-reset-token")]
        public async Task<ActionResult> VerifyResetToken([FromQuery] string email, [FromQuery] string token)
        {
            var res = await _userService.VerifyResetToken(email, token);
            return StatusCode((int)res.statusCode, res);
        }

    }
}