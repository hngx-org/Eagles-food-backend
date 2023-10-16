using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Filters;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Helpers;
using eagles_food_backend.Services.EmailService;
using eagles_food_backend.Services.OrganizationRepository;
using eagles_food_backend.Services.UriService;

using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly LunchDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthenticationClass _authentication;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUriService _uriService;

        public OrganizationService(LunchDbContext context, IMapper mapper, AuthenticationClass authentication,
            IHttpContextAccessor httpContextAccessor, IEmailService emailService, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _authentication = authentication;
            _emailService = emailService;
            _uriService = uriService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<Dictionary<string, string>>> CreateStaffMember(CreateStaffDTO model)
        {
            Response<Dictionary<string, string>> response = new();
            User? newUser = _mapper.Map<User>(model);
            newUser.IsAdmin = true;

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

            var newOrg = new Organization()
            {
                Name = model.OrgName,
                CurrencyCode = "₦",
                LunchPrice = model.OrgLunchPrice == 0 ? 1000 : model.OrgLunchPrice,
                Hidden = false
            };

            if (_context.Organizations.Any(x => x.Name == newOrg.Name && x.IsDeleted == false))
            {
                return new Response<Dictionary<string, string>>()
                {
                    success = false,
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Organization already exists",
                    data = null
                };
            }
            await _context.AddAsync(newOrg);
            await _context.SaveChangesAsync();

            var newWallet = new OrganizationLunchWallet()
            {
                OrgId = newOrg.Id,
                Balance = 0
            };

            await _context.AddAsync(newWallet);
            await _context.SaveChangesAsync();



            // find eagles org
            var resOrg = await _context.Organizations.FirstAsync(o => o.Name == newOrg.Name);

            // store in db
            try
            {
                _authentication.CreatePasswordHash(model.Password, out string password_hash);

                newUser.PasswordHash = password_hash;
                newUser.IsAdmin = true;
                newUser.Org = resOrg;
                newUser.OrgId = resOrg.Id;
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

                response.message = "Staff/Organization created up successfully";
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

        //Get all Organization Invites
        public async Task<Response<List<OrganizationInvitationDTO>>> OrganizationInvites(int userId, PaginationFilter validFilter)
        {
            var route = _httpContextAccessor.HttpContext.Request.Path.Value;
            Response<List<OrganizationInvitationDTO>> response = new();
            User? user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = default;
                return response;
            }
            if ((bool)!user.IsAdmin || user.IsAdmin == null)
            {
                response.success = false;
                response.message = "You do not have access to this resource";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = default;
                return response;
            }
            var organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Id == user.OrgId);
            if (organization is null)
            {
                response.success = false;
                response.message = "Organization not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = default;
                return response;
            }
            List<User>? users = await _context.Users.Where(x => x.Email != user.Email && x.IsAdmin == false).ToListAsync();
            var organizationInviteRequestQuery = _context.OrganizationInvites
                .Where(x => x.OrgId == user.OrgId)
                .Select(x => new OrganizationInvitationDTO()
                {
                    CreatedAt = x.CreatedAt,
                    Id = x.Id,
                    OrgId = organization.Id,
                    Org = organization.Name,
                    Email = x.Email,
                    Status = x.Status
                });

            var organizationInviteRequest = await organizationInviteRequestQuery
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            var orgsCount = await organizationInviteRequestQuery.CountAsync();
            return PaginationHelper.CreatePagedReponse(organizationInviteRequest, validFilter, orgsCount, _uriService, route, message: orgsCount > 0 ? "Invites Fetched Successfuuly" : "You have any Invitations");
        }

        //Get all Organization Invite Request from User
        public async Task<Response<List<OrganizationInvitationDTO>>> OrganizationInviteRequests(int userId, PaginationFilter validFilter)
        {
            var route = _httpContextAccessor.HttpContext.Request.Path.Value;
            Response<List<OrganizationInvitationDTO>> response = new();
            User? user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = default;
                return response;
            }
            if ((bool)!user.IsAdmin || user.IsAdmin == null)
            {
                response.success = false;
                response.message = "You do not have access to this resource";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = default;
                return response;
            }
            var organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Id == user.OrgId);
            if (organization is null)
            {
                response.success = false;
                response.message = "Organization not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = default;
                return response;
            }
            List<User>? users = await _context.Users.Where(x => x.Email != user.Email && x.IsAdmin == false).ToListAsync();
            var organizationInviteRequestQuery = _context.InvitationRequests
                .Where(x => x.OrgId == user.OrgId && x.Status != true)
                .Select(x => new OrganizationInvitationDTO()
                {
                    CreatedAt = x.CreatedAt,
                    Id = x.Id,
                    OrgId = organization.Id,
                    Org = organization.Name,
                    Email = x.UserEmail,
                    Status = x.Status
                });

            var organizationInviteRequest = await organizationInviteRequestQuery
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var orgsCount = await organizationInviteRequestQuery.CountAsync();
            return PaginationHelper.CreatePagedReponse(organizationInviteRequest, validFilter, orgsCount, _uriService, route, message: "Invites returned successfully");
        }

        public async Task<Response<bool>> ToggleInviteRequest(int userId, ToggleInviteDTO model)
        {
            Response<bool> response = new();
            User? loggedInuser = await _context.Users.FindAsync(userId);
            if (loggedInuser is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            var invite = await _context.InvitationRequests.Where(x => x.Id == model.InviteId && x.IsDeleted == false).FirstOrDefaultAsync();
            var organization = await _context.Organizations.FindAsync(invite?.OrgId);
            if (invite is null || organization is null)
            {
                response.success = false;
                response.message = "Invite Request not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            User? user = await _context.Users.Where(x => x.Email == invite.UserEmail).FirstOrDefaultAsync();
            if (user is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            if ((bool)invite.Status)
            {
                response.success = true;
                response.message = "Invite Already Accepted";
                response.statusCode = HttpStatusCode.OK;
                response.data = true;
                return response;
            }
            invite.Status = model.Status;
            if (model.Status)
            {
                user.Org = organization;
                user.OrgId = organization.Id;
                _context.Update(user);
            }

            await _context.SaveChangesAsync();
            response.message = "Successful Operation";
            response.statusCode = HttpStatusCode.OK;
            response.data = true;
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
                // make sure email is unique in invites
                //if (await _context.OrganizationInvites.AnyAsync(i => i.Email == model.Email))
                //{
                //    response.success = false;
                //    response.message = "User has already been invited before";
                //    response.data = new Dictionary<string, string>() {
                //        { "email", model.Email }
                //    };
                //    response.statusCode = HttpStatusCode.BadRequest;

                //    return response;
                //}

                var orgID = admin.OrgId;
                var org = await _context.Organizations.Where(o => o.Id == orgID).FirstAsync();

                if (org is null)
                {
                    response.success = false;
                    response.message = "Organization does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }
                var token = Guid.NewGuid().ToString();
                var invite = new OrganizationInvite()
                {
                    Email = model.Email,
                    OrgId = org.Id,
                    Ttl = DateTime.Now.AddDays(1),
                    Token = token
                };
                var invitee = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (invitee is null)
                {
                    _emailService.SendEmail(new MailData()
                    {
                        EmailBody = $"Hi {model.Email}, You have received an invite from {org.Name} to join their lunch Space. Use this code when you register {token}",
                        EmailSubject = "Invitation to SignUp on Lunch",
                        EmailToId = model.Email,
                        EmailToName = model.Email
                    });
                }


                await _context.OrganizationInvites.AddAsync(invite);
                await _context.SaveChangesAsync();

                response.success = true;
                response.data = null;

                response.message = "Organisation Invite sent out successfully";
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

        public async Task<Response<string>> HideOrganization(int userId, bool hide)
        {
            Response<string> response = new();
            User? user = await _context.Users.FindAsync(userId);

            try
            {
                // ensure user exists
                if (user is null)
                {
                    response.success = false;
                    response.message = "User not found";
                    response.statusCode = HttpStatusCode.NotFound;
                    response.data = null;
                    return response;
                }

                // make sure they're an admin
                if (user.IsAdmin != true)
                {
                    response.success = false;
                    response.message = "User unauthorised";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.data = null;
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
                org.Hidden = hide;
                _context.Update(org);
                await _context.SaveChangesAsync();
                response.success = true;
                response.message = "Successful Operation";
                response.statusCode = HttpStatusCode.OK;
                response.data = "Success";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.statusCode = HttpStatusCode.InternalServerError;
                response.success = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<OrganizationDTO>> GetOrganization(int userId)
        {
            Response<OrganizationDTO> response = new();
            User? user = await _context.Users.FindAsync(userId);

            try
            {
                // ensure user exists
                if (user is null)
                {
                    response.success = false;
                    response.message = "User not found";
                    response.statusCode = HttpStatusCode.NotFound;
                    response.data = null;
                    return response;
                }

                // make sure they're an admin
                if (user.IsAdmin != true)
                {
                    response.success = false;
                    response.message = "User unauthorised";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.data = null;
                    return response;
                }

                var orgID = user.OrgId;
                var org = await _context.Organizations.FindAsync(orgID);

                if (org is null || user.OrgId != org.Id)
                {
                    response.success = false;
                    response.message = "Organisation does not exist";
                    response.statusCode = HttpStatusCode.BadRequest;

                    return response;
                }

                var orgResponse = new OrganizationDTO()
                {
                    Name = org.Name,
                    Currency = org.CurrencyCode,
                    LunchPrice = org.LunchPrice,
                    Hidden = org.Hidden
                };
                response.success = true;
                response.message = "Successful Operation";
                response.statusCode = HttpStatusCode.OK;
                response.data = orgResponse;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.statusCode = HttpStatusCode.InternalServerError;
                response.success = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<List<OrganizationReadDTO>>> GetAllOrganizations(PaginationFilter validFilter, string? searchTerm)
        {
            try
            {
                searchTerm = searchTerm?.Trim().ToLower();
                var route = _httpContextAccessor.HttpContext.Request.Path.Value;
                var orgsQuery = _context.Organizations
                    .Where(x => x.IsDeleted == false
                    && x.Hidden == false
                    && (string.IsNullOrEmpty(searchTerm) ? true : x.Name.Contains(searchTerm)));
                var orgs = await orgsQuery
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                 .Take(validFilter.PageSize)
                 .ToListAsync();
                var orgsCount = await orgsQuery.CountAsync();
                var orgsDTO = _mapper.Map<List<OrganizationReadDTO>>(orgs);
                return PaginationHelper.CreatePagedReponse(orgsDTO, validFilter, orgsCount, _uriService, route, message: "Organizations returned successfully", searchTerm: searchTerm);
            }
            catch (Exception)
            {
                return new Response<List<OrganizationReadDTO>>()
                {
                    data = null,
                    success = false,
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Internal Server Error"
                };
            }

        }
    }
}