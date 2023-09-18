using Microsoft.AspNetCore.Mvc;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LunchController : ControllerBase
    {
        private readonly ILogger<LunchController> _logger;

        public LunchController(ILogger<LunchController> logger)
        {
            _logger = logger;
        }
    }
}