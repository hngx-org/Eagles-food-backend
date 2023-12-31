﻿using Asp.Versioning;

using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Filters;
using eagles_food_backend.Services.LunchRepository;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/lunches")]
    [ApiVersion(1.0)]
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
        [HttpPost]
        public async Task<ActionResult> CreateLunch([FromBody] CreateLunchDTO model)
        {
            var response = await _lunchService.create(model);
            return StatusCode((int)response.statusCode, response);
        }

        /// <summary>
        /// Gets all lunch
        /// </summary>
        /// <returns>A response with 200 and a list of the lunches</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllLunches([FromQuery] PaginationFilter filter)
        {

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var response = await _lunchService.getAll(validFilter);
            return StatusCode((int)response.statusCode, response);
        }


        /// <summary>
        /// Gets all lunch leaderboard data
        /// </summary>
        /// <returns>A response with 200 and a list of the lunches</returns>
        [HttpGet("leaderboard")]
        public async Task<ActionResult> GetLeaderboard([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var response = await _lunchService.Leaderboard(validFilter);
            return StatusCode((int)response.statusCode, response);
        }

        /// <summary>
        /// Get lunch balance of logged in user
        /// </summary>
        [HttpGet("lunch-balance")]
        public async Task<ActionResult> GetLunchBalance()
        {
            var response = await _lunchService.GetLunchBalance();
            return StatusCode((int)response.statusCode, response);
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
            return StatusCode((int)response.statusCode, response);
        }

        /// <summary>
        /// withdraws from gifted free lunches
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/lunch/withdrawlunch
        /// {
        ///     "quantity": 5,
        ///     
        /// }
        /// </code>
        /// </remarks>
        /// <param name="withdrawDTO">Request body containing the quantity to bw withdrawn</param>
        /// <returns>A response with 200 and withdrawal amount</returns>
        [HttpPost("withdraw-lunch")]
        public async Task<ActionResult> WithdrawLunch([FromBody] WithdrawLunchDTO withdrawDTO)
        {
            var response = await _lunchService.withdrawLunch(withdrawDTO);
            return StatusCode((int)response.statusCode, response);
        }
    }
}