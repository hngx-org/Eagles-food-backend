using System.Security.Claims;
using Asp.Versioning;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Filters;
using eagles_food_backend.Services.OrganizationRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eagles_food_backend.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/organization")]
    [ApiVersion(1.0)]
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
        /// Get an organization
        /// </summary>
        /// <returns>The Organization of the Logged in User</returns>
        /// <response code="200">Returns the Organization of Logged In user</response>
        [HttpGet]
        public async Task<IActionResult> GetOrganization()
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "admin")
            {
                return Unauthorized();
            }

            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var response = await _organizationService.GetOrganization(id);
                return StatusCode((int)response.statusCode, response);
            }
            else
            {
                return BadRequest();
            }
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
        /// <returns>A response with the id, email, and orgID of the new staff and token</returns>
        /// <response code="201">Returns the id, email, orgID of the new staff and token</response>
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

        /// <summary>
        /// Updates the wallet of an organisation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///  <code>
        ///  PATCH api/organizations/wallet
        ///  {
        ///  "amount": "200"
        ///  }
        ///  </code>
        ///  </remarks>
        ///  <param name="model">The request body with the details</param>
        ///  <returns>nothing</returns>
        ///  <response code="200">Returns nothing</response>
        ///  <response code="400">If valudation fails</response>
        ///  <response code="404">If unauthorised</response>
        ///  <response code="500">If there was an error updating</response>
        ///  <response code="401">If unauthorised</response>
        [HttpPatch("wallet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateOrganizationWallet([FromBody] UpdateOrganizationWalletDTO model)
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "admin")
            {
                return Unauthorized();
            }

            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var res = await _organizationService.UpdateOrganizationWallet(id, model);
                return StatusCode((int)res.statusCode, res);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates the lunch price of an organisation
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        /// <code>
        /// PATCH api/organizations/lunch/update
        /// {
        /// "lunchPrice": "200"
        /// }
        /// </code>
        /// </remarks>
        ///  <param name="model">The request body with the details</param>
        ///  <returns>nothing</returns>
        ///  <response code="200">Returns nothing</response>
        ///  <response code="400">If valudation fails</response>
        ///  <response code="404">If unauthorised</response>
        ///  <response code="500">If there was an error updating</response>
        ///  <response code="401">If unauthorised</response>
        [HttpPatch("lunch/update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> UpdateOrganizationLunchPrice([FromBody] UpdateOrganizationLunchPriceDTO model)
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "admin")
            {
                return Unauthorized();
            }

            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var res = await _organizationService.UpdateOrganizationLunchPrice(id, model);
                return StatusCode((int)res.statusCode, res);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds a user to an organisation
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/organizations/invite
        /// {
        /// "email": "john@doe"
        /// }
        /// </code>
        /// </remarks>
        ///  <param name="model">The request body with the details</param>
        ///  <returns>nothing</returns>
        ///  <response code="200">Returns nothing</response>
        ///  <response code="400">If valudation fails</response>
        ///  <response code="404">If unauthorised</response>
        ///  <response code="500">If there was an error </response>
        ///  <response code="401">If unauthorised</response>
        [HttpPost("invite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InviteToOrganization([FromBody] InviteToOrganizationDTO model)
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "admin")
            {
                return Unauthorized();
            }

            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var res = await _organizationService.InviteToOrganization(id, model);
                return StatusCode((int)res.statusCode, res);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This end point is for orgnizations to see all thier invites
        /// </summary>
        /// <returns>It returns all the invites a person has unattended to </returns>
        /// <response code="200">Returns the user</response>
        [HttpGet("organizationinvites")]
        public async Task<IActionResult> GetUserInvites([FromQuery] PaginationFilter filter)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SearchTerm);
                var response = await _organizationService.OrganizationInvites(id, validFilter);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }


        /// <summary>
        /// This end point is for organization to see all invites request for thier account
        /// </summary>
        /// <returns>It returns all the invites a person has unattended to </returns>
        /// <response code="200">Returns the user</response>
        [HttpGet("organizationinviterequest")]
        public async Task<IActionResult> GetUserInviteRequests([FromQuery] PaginationFilter filter)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var response = await _organizationService.OrganizationInviteRequests(id, validFilter);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }

        /// <summary>
        /// This end point is for organization to accept invite Request
        /// send true for yes(acceptance) and false for no(rejection)
        /// </summary>
        /// <returns>it returns the state of the Invite Request </returns>
        /// <response code="200">Returns the user</response>
        [HttpPost("toggleinvite")]
        public async Task<IActionResult> Invite([FromBody] ToggleInviteDTO model)
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var response = await _organizationService.ToggleInviteRequest(id, model);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }

        /// <summary>
        /// Hides an Organization
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST api/organizations/{status}
        /// </code>
        /// </remarks>
        ///  <param name="model">The request body with the details</param>
        ///  <returns>nothing</returns>
        ///  <response code="200">Returns success</response>
        ///  <response code="400">If valudation fails</response>
        ///  <response code="404">If unauthorised</response>
        ///  <response code="500">If there was an error </response>
        ///  <response code="401">If unauthorised</response>
        [HttpGet("invite/{hide}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> HideOrganization(bool hide)
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "admin")
            {
                return Unauthorized();
            }

            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var res = await _organizationService.HideOrganization(id, hide);
                return StatusCode((int)res.statusCode, res);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all organizations
        /// </summary>
        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllOrganizations([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SearchTerm);
            var response = await _organizationService.GetAllOrganizations(validFilter);
            return StatusCode((int)response.statusCode, response);
        }

        /// <summary>
        /// Leave the organization of the authenticated user
        /// </summary>
        [HttpGet("leave")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LeaveOrganization()
        {
            if (int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value, out int id))
            {
                var response = await _organizationService.LeaveOrganization(id);
                return StatusCode((int)response.statusCode, response);
            }
            else return BadRequest();
        }
    }
}
