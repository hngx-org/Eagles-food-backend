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

        [HttpPost("send")]
        public async Task<ActionResult> CreateLunch([FromBody] CreateLunchDTO model)
        {
            var response = await _lunchService.create(model);
            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllLunches()
        {
            var response = await _lunchService.getAll();
            return Ok(response);
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult> GetLunchById(int id)
        {
            var response = await _lunchService.getById(id);
            return Ok(response);
        }
    }
}
