using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Services.LunchRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("all/{userId}")]
        public async Task<ActionResult> GetAllLunches(int userId)
        {
            var response = await _lunchService.getAll(userId);

            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult> GetLunchById(int id)
        {
            var response = await _lunchService.getById(id);

            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
