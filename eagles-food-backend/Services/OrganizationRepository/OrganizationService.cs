using System.ComponentModel.DataAnnotations;
using System.Net;
using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Services.OrganizationRepository;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly LunchDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthenticationClass _authentication;

        public OrganizationService(LunchDbContext context, IMapper mapper, AuthenticationClass authentication)
        {
            _context = context;
            _mapper = mapper;
            _authentication = authentication;
        }

        // make a new empty org, add this staff member as the admin
        public async Task<Response<Dictionary<string, string>>> CreateStaffMember(CreateStaffDTO model)
        {
            Response<Dictionary<string, string>> response = new();
            User? newUser = _mapper.Map<User>(model);

            // ensure it's an actual (plausible) email
            if (!new EmailAddressAttribute().IsValid(newUser.Email))
            {
                response.success = false;
                response.message = "Invalid email";
                response.data = new Dictionary<string, string>() {
                    { "email", newUser.Email }
                };
                response.statusCode = HttpStatusCode.BadRequest;

                return response;
            }

            // ensure email is unique
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                response.success = false;
                response.message = "Email already exists";
                response.data = new Dictionary<string, string>() {
                    { "email", newUser.Email }
                };
                response.statusCode = HttpStatusCode.BadRequest;

                return response;
            }

            // make their org.
            var newOrg = new Organization()
            {
                Name = model.FirstName + " " + model.LastName + "'s Organization",
                CurrencyCode = "₦",
                LunchPrice = 1000
            };

            await _context.AddAsync(newOrg);
            await _context.SaveChangesAsync();

            // store in db
            try
            {
                _authentication.CreatePasswordHash(model.Password, out string password_hash);

                newUser.PasswordHash = password_hash;
                newUser.IsAdmin = true;
                newUser.OrgId = newOrg.Id;

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                response.success = true;
                response.data = new Dictionary<string, string>() {
                    { "id", newUser.Id.ToString() },
                    { "orgId", newUser.OrgId.ToString()! },
                    { "email", newUser.Email }
                };
                response.message = "Staff signed up successfully";
                response.statusCode = HttpStatusCode.Created;
            }

            // catch any errors
            catch (Exception ex)
            {
                response.statusCode = HttpStatusCode.InternalServerError;
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }

        // modify the organisation of user with id
        public async Task<Response<Dictionary<string, string>>> ModifyOrganization(int UserID, ModifyOrganizationDTO model)
        {
            Response<Dictionary<string, string>> response = new();
            User? user = await _context.Users.FindAsync(UserID);

            try
            {
                // ensure user exists
                if (user is null)
                {
                    response.success = false;
                    response.message = "User not found";
                    response.statusCode = HttpStatusCode.NotFound;
                    response.data = new Dictionary<string, string>() {
                        { "id", UserID.ToString() }
                    };

                    return response;
                }

                // make sure they're an admin
                if (user.IsAdmin != true)
                {
                    response.success = false;
                    response.message = "User unauthorised";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.data = new Dictionary<string, string>() {
                        { "id", UserID.ToString() }
                    };

                    return response;
                }

                var orgID = user.OrgId;
                var org = await _context.Organizations.FindAsync(orgID);

                if (org is null) {
                    response.success = false;
                    response.message = "Organisation does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                _context.Entry(org).State = EntityState.Modified;

                org.LunchPrice = model.LunchPrice;
                org.CurrencyCode = model.Currency;
                org.Name = model.OrganizationName;

                await _context.SaveChangesAsync();

                response.success = true;
                response.data = new Dictionary<string, string>() {
                    { "orgId", org.Id.ToString() },
                    { "name", org.Name },
                    { "lunchPrice", org.LunchPrice.ToString() },
                    { "currency", org.CurrencyCode }
                };
                response.message = "Organisation modified successfully";
                response.statusCode = HttpStatusCode.OK;
            }

            // catch any errors
            catch (Exception ex)
            {
                response.statusCode = HttpStatusCode.InternalServerError;
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }

        //     public async Task<ServiceResponse<OrganizationDTO>> GetOrganization(int id)
        //     {
        //         var organization = await _context.Organizations.FindAsync(id);
        //         if (organization == null) return _responseService.ErrorResponse<OrganizationDTO>("Organization not found");
        //         var response = new OrganizationDTO()
        //         {
        //             Currency = organization.CurrencyCode,
        //             LunchPrice = organization.LunchPrice,
        //             Name = organization.Name,
        //         };
        //         return _responseService.SuccessResponse(response);
        //     }
    }
}
