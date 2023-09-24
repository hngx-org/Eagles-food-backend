using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Services.LunchRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/lunch")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LunchController : ControllerBase
    {
        private readonly ILunchRepository _lunchService;

        public LunchController(ILunchRepository lunchService)
        {
            _lunchService = lunchService;
        }

        /// <summary>
        /// Send Lunch to a user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/lunch/send
        /// {
        ///     "receivers": [1, 2, 3],
        ///     "quantity": 5,
        ///     "note": "This is a note for the lunch request."
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">Request body containing a note, the quantity and the receivers id</param>
        /// <returns>A response with 201 and a message</returns>
        [HttpPost("send")]
        public async Task<ActionResult> CreateLunch([FromBody] CreateLunchDTO model)
        {
            var response = await _lunchService.create(model);
            return Ok(response);
        }

        /// <summary>
        /// Gets all lunch
        /// </summary>
        /// <returns>A response with 200 and a list of the lunches</returns>
        [HttpGet("all")]
        public async Task<ActionResult> GetAllLunches()
        {
            var response = await _lunchService.getAll();
            return Ok(response);
        }

        /// <summary>
        /// Get a lunch by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A reponse with 200 and the lunch details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLunchById(int id)
        {
            var response = await _lunchService.getById(id);
            return Ok(response);
        }

        [HttpPost("withdrawlunch")]
        public async Task<ActionResult> WithdrawLunch([FromBody] WithdrawLunchDTO withdrawDTO)
        {
            return Ok(await _lunchService.withdrawLunch(withdrawDTO));
        }
    }
}
