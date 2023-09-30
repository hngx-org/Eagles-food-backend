using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

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

        // make a eagles org if non-existent, add this staff member as an admin
        public async Task<Response<Dictionary<string, string>>> CreateStaffMember(CreateStaffDTO model)
        {
            Response<Dictionary<string, string>> response = new();
            User? newUser = _mapper.Map<User>(model);

            // make eagles org if non-existent
            if (!await _context.Organizations.AnyAsync(o => o.Name == "Eagles Food"))
            {
                var EaglesOrg = new Organization()
                {
                    Name = "Eagles Food",
                    CurrencyCode = "₦",
                    LunchPrice = 1000
                };

                await _context.AddAsync(EaglesOrg);
                await _context.SaveChangesAsync();

                var EaglesWallet = new OrganizationLunchWallet()
                {
                    OrgId = EaglesOrg.Id,
                    Balance = 0
                };

                await _context.AddAsync(EaglesWallet);
                await _context.SaveChangesAsync();
            }

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

            // find eagles org
            var eaglesOrg = await _context.Organizations.FirstAsync(o => o.Name == "Eagles Food");

            // store in db
            try
            {
                _authentication.CreatePasswordHash(model.Password, out string password_hash);

                newUser.PasswordHash = password_hash;
                newUser.IsAdmin = true;
                newUser.Org = eaglesOrg;
                newUser.OrgId = eaglesOrg.Id;
                newUser.LunchCreditBalance = 0;

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                // get role (even if it's null) and create token
                var role = newUser.IsAdmin == true ? "admin" : "user";
                var token = _authentication.createToken(newUser.Id.ToString(), role);

                response.success = true;

                // get user data and add token
                var res = newUser
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => Convert.ToString(prop.GetValue(newUser, null)));

                // remove sensitive data
                res.Remove("PasswordHash");
                res.Remove("LunchReceivers");
                res.Remove("LunchSenders");
                res.Remove("Withdrawals");
                res.Remove("IsDeleted");
                res.Remove("Org");

                res.Add("organization_name", newUser.Org.Name);
                res.Add("access_token", token);

                response.data = res!;

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

                if (org is null)
                {
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

        // add money to the organization's wallet
        public async Task<Response<Dictionary<string, string>>> UpdateOrganizationWallet(int UserID, UpdateOrganizationWalletDTO model)
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

                if (org is null)
                {
                    response.success = false;
                    response.message = "Organisation does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                var wallet = _context.OrganizationLunchWallets.Where(w => w.OrgId == orgID).FirstOrDefault();

                if (wallet is null)
                {
                    response.success = false;
                    response.message = "Wallet does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                wallet.Balance += model.amount;

                await _context.SaveChangesAsync();

                response.success = true;
                response.data = null;

                response.message = "Organisation wallet updated successfully";
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

        // update the price of lunch for the organization
        public async Task<Response<Dictionary<string, string>>> UpdateOrganizationLunchPrice(int UserID, UpdateOrganizationLunchPriceDTO model)
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

                if (org is null)
                {
                    response.success = false;
                    response.message = "Organisation does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                _context.Entry(org).State = EntityState.Modified;

                org.LunchPrice = model.LunchPrice;

                await _context.SaveChangesAsync();

                response.success = true;
                response.data = null;

                response.message = "Organisation lunch price updated successfully";
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

        // invite to the organization
        public async Task<Response<Dictionary<string, string>>> InviteToOrganization(int UserID, InviteToOrganizationDTO model)
        {
            Response<Dictionary<string, string>> response = new();
            User? admin = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserID);

            try
            {
                // ensure user exists
                if (admin is null)
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
                if (admin.IsAdmin != true)
                {
                    response.success = false;
                    response.message = "User unauthorised";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.data = new Dictionary<string, string>() {
                        { "id", UserID.ToString() }
                    };

                    return response;
                }

                var invitee = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (invitee is null)
                {
                    response.success = false;
                    response.message = "User does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                // make sure email is unique in invites
                if (await _context.OrganizationInvites.AnyAsync(i => i.Email == model.Email))
                {
                    response.success = false;
                    response.message = "User has already been invited before";
                    response.data = new Dictionary<string, string>() {
                        { "email", model.Email }
                    };
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                var orgID = admin.OrgId;
                var org = await _context.Organizations.Where(o => o.Id == orgID).FirstAsync();

                if (org is null)
                {
                    response.success = false;
                    response.message = "Organisation does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                var invite = new OrganizationInvite()
                {
                    Email = model.Email,
                    OrgId = org.Id,
                    Ttl = DateTime.Now.AddDays(1),
                    Token = Guid.NewGuid().ToString()
                };

                invitee.OrgId = org.Id;
                invitee.Org = org;

                await _context.OrganizationInvites.AddAsync(invite);
                await _context.SaveChangesAsync();

                response.success = true;
                response.data = null;

                response.message = "User Added To Organisation successfully";
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


    }
}