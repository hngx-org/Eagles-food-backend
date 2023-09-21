using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Services.LunchRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [HttpPost()]
        [ProducesResponseType(typeof(Response<ResponseLunchDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateLunch([FromBody] CreateLunchDTO model)
        {
            var res = await _lunchService.create(model);

            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(Response<List<ResponseLunchDTO>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllLunches()
        {
            var res = await _lunchService.getAll();

            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
