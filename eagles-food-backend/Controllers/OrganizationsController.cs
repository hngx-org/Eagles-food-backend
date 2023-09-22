using System.Security.Claims;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Services;
using eagles_food_backend.Services.OrganizationRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/organization")]
    [Produces("application/json")]
    public class OrganizationsController : ControllerBase
    {
        private readonly ILogger<OrganizationsController> _logger;
        private readonly IOrganizationService _organizationService;

        public OrganizationsController(ILogger<OrganizationsController> logger, IOrganizationService organizationService)
        {
            _logger = logger;
            _organizationService = organizationService;
        }

        /// <summary>
        /// Makes a org. and a admin staff member
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/organization/staff/signup
        /// {
        ///   "lastName": "john",
        ///   "firstName": "doe",
        ///   "email": "john@doe.com",
        ///   "address": "123 Main St.",
        ///   "password": "pass",
        ///   "phone": "123456"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">Request body with staff's details</param>
        /// <returns>A response with the id, email, and orgID of the new staff</returns>
        /// <response code="201">Returns the id, email, orgID of the new staff</response>
        /// <response code="400">If validation errors</response>
        /// <response code="500">If there was an error creating the staff</response>        
        [HttpPost("staff/signup")]
        public async Task<IActionResult> CreateStaffMember([FromBody] CreateStaffDTO model)
        {
            var res = await _organizationService.CreateStaffMember(model);
            return StatusCode((int)res.statusCode, res);
        }

        /// <summary>
        /// Modifies an existing organisation
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/organizations/modify
        /// {
	    ///   "organisationName": "Big Bank PLC",
	    ///   "lunchPrice": "200",
        ///   "currency": "$"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The request body with the details</param>
        /// <returns>a response with the id, and details</returns>
        /// <response code="200">Returns the modified details</response>
        /// <response code="400">If staff doesnt belong to an org</response>
        /// <response code="404">If unauthorised</response>        
        /// <response code="500">If there was an error creating the staff</response>        
        [HttpPut("modify")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ModifyOrganization([FromBody] ModifyOrganizationDTO model)
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "admin")
            {
                return Unauthorized();
            }
            
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var res = await _organizationService.ModifyOrganization(id, model);
                return StatusCode((int)res.statusCode, res);
            }
            else
            {
                return BadRequest();
            }
        }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetOrganization(int id)
        // {
        //     var org = await _organizationService.GetOrganization(id);
        //     return Ok(org);
        // }
    }
}