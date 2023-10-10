using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Services.EmailService;

using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services.UserServices
{
    public class UserService : IUserRepository
    {
        private readonly LunchDbContext db_context;
        private readonly IMapper mapper;
        private readonly AuthenticationClass authentication;
        private readonly IEmailService _emailService;
        public readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(LunchDbContext db_context,
            IMapper mapper,
            AuthenticationClass authentication,
            IEmailService emailService,
            IConfiguration config,
            IWebHostEnvironment webHostEnvironment)
        {
            this.db_context = db_context;
            this.mapper = mapper;
            this.authentication = authentication;
            this._emailService = emailService;
            this._config = config;
            _webHostEnvironment = webHostEnvironment;
        }

        private CreateBankDTO GenerateBankDetails() //Generates new account number for each created user
        {

            var usersCount = db_context.Users.ToList().Count;
            var test = db_context.Users.Any(m => m.BankNumber == "100000000");

            if (usersCount < 1)
            {
                CreateBankDTO newUserBankDetails = new()
                {
                    BankNumber = "100000000",
                    BankCode = "257801",
                    BankName = "FLC",
                    BankRegion = "Jupiter"
                };

                return newUserBankDetails;
            }

            var bankNumber = usersCount + 100000000;


            CreateBankDTO userBankDetails = new()
            {
                BankNumber = bankNumber.ToString(),
                BankCode = "257801",
                BankName = "FLC",
                BankRegion = "Jupiter"
            };

            return userBankDetails;
        }

        // create a new user
        public async Task<Response<Dictionary<string, string>>> CreateUser(CreateUserDTO user)
        {
            Response<Dictionary<string, string>> response = new();
            User? newUser = mapper.Map<User>(user);
            // ensure it's an actual (plausible) email
            if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                response.success = false;
                response.message = "Invalid email";
                response.data = new Dictionary<string, string>() {
                    { "email", user.Email }
                };
                response.statusCode = HttpStatusCode.BadRequest;

                return response;
            }

            // ensure email is unique
            if (await db_context.Users.AnyAsync(u => u.Email == user.Email))
            {
                response.success = false;
                response.message = "Email already exists";
                response.data = new Dictionary<string, string>() {
                    { "email", user.Email }
                };
                response.statusCode = HttpStatusCode.BadRequest;

                return response;
            }

          
            // find eagles org
            var org = await db_context.Organizations.FirstAsync(o => o.Name == "Eagles Food");

            // store in db
            try
            {
                authentication.CreatePasswordHash(user.Password, out string password_hash);

                newUser.PasswordHash = password_hash;
                newUser.IsAdmin = false;
                newUser.LunchCreditBalance = 100;

                var generatedDetails = GenerateBankDetails();

                newUser.BankName = generatedDetails.BankName;
                newUser.BankNumber = generatedDetails.BankNumber;
                newUser.BankCode = generatedDetails.BankCode;
                newUser.BankRegion = generatedDetails.BankRegion;
                if (user.InviteCode != null)
                {
                    var invitation = db_context.OrganizationInvites.Where(x=>x.Token == user.InviteCode && x.Email == user.Email).FirstOrDefault();
                    if(invitation == null)
                    {
                        response.success = false;
                        response.message = "Invalid Invite Code";
                        response.data = new Dictionary<string, string>() {
                    { "email", user.Email }
                };
                        response.statusCode = HttpStatusCode.BadRequest;

                        return response;
                    }
                    newUser.OrgId = invitation.OrgId;
                    invitation.Status = true;
                    db_context.Update(invitation);
                }

                await db_context.Users.AddAsync(newUser);
                await db_context.SaveChangesAsync();

                response.success = true;

                // get role (even if it's null) and create token
                var role = newUser.IsAdmin == true ? "admin" : "user";
                var token = authentication.createToken(newUser.Id.ToString(), role);

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

                res.Add("access_token", token);

                response.data = res!;
                response.message = "User signed up successfully";
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

        // login a user
        public async Task<Response<Dictionary<string, string>>> Login(UserLoginDTO user)
        {
            Response<Dictionary<string, string>> response = new();
            User? user_login = await db_context.Users.Where(u => u.Email == user.Email).Include(x => x.Org).FirstOrDefaultAsync();

            var userindb = mapper.Map<CreateUserDTO>(user_login);

            // ensure user exists
            if (user_login is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.Unauthorized;
                response.data = new Dictionary<string, string>() {
                    { "email", user.Email }
                };

                return response;
            }

            // ensure password is correct
            try
            {
                var passwordIsValid = authentication.verifyPasswordHash(user.Password, user_login.PasswordHash);

                if (!passwordIsValid)
                {
                    response.success = false;
                    response.message = "Incorrect password";
                    response.statusCode = HttpStatusCode.Unauthorized;
                }
                else
                {
                    // get role (even if it's null) and create token
                    var role = user_login.IsAdmin == true ? "admin" : "user";
                    var token = authentication.createToken(user_login.Id.ToString(), role);

                    response.success = true;

                    // get user data and add token
                    var res = user_login
                        .GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(prop => prop.Name, prop => Convert.ToString(prop.GetValue(user_login, null)));

                    // remove sensitive data
                    res.Remove("PasswordHash");
                    res.Remove("LunchReceivers");
                    res.Remove("LunchSenders");
                    res.Remove("Withdrawals");
                    res.Remove("IsDeleted");
                    res.Remove("Org");

                    res.Add("access_token", token);
                    res.Add("organization_name", user_login.Org?.Name ?? "Default Organization");

                    response.data = res!;
                    response.message = "User authenticated successfully";
                    response.statusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                response.statusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<Response<bool>> UploadPhoto(IFormFile photo, int id)
        {
            Response<bool> response = new();
            User? user = await db_context.Users.FindAsync(id);
            if (user is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.BadRequest;
                return response;
            }
            var allowedFormats = new List<string>() { "image/jpeg", "image/png" };
            if (!allowedFormats.Contains(photo.ContentType))
            {
                response.success = false;
                response.message = "Invalid File Format";
                response.statusCode = HttpStatusCode.BadRequest;
                return response;
            }
            if ((photo.Length / (1024.0 * 1024.0)) > 1.0)
            {
                response.success = false;
                response.message = "Image cannot be larger than 1MB";
                response.statusCode = HttpStatusCode.BadRequest;
                return response;
            }
            if (!Directory.Exists("storage"))
            {
                Directory.CreateDirectory("storage");
            }
            var photoExtension = Path.GetExtension(photo.FileName);
            var photoNewName = $"{user.Email}{photoExtension}";
            var webHostPath =  _webHostEnvironment.ContentRootPath;
            var path = Path.Combine("storage", photoNewName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            user.ProfilePic = webHostPath+path;
            await db_context.SaveChangesAsync();
            response.success = true;
            response.data = true;
            response.message = "User Photo updated successfully";
            response.statusCode = HttpStatusCode.OK;
            return response;
        }

        public async Task<Response<List<OrganizationInviteDTO>>> UserInvites(int userId)
        {
            Response<List<OrganizationInviteDTO>> response = new();
            User? user = await db_context.Users.FindAsync(userId);
            if (user is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = default;
                return response;
            }
            var userinvites = await db_context.OrganizationInvites.Where(x => x.Email == user.Email && x.IsDeleted == false && x.Status == null).Include(x => x.Org).ToListAsync();
            response.success = true;
            response.message = userinvites.Count > 0 ? "Invites Fetched Successfuuly" : "You dont have any Pending Invites";
            response.statusCode = HttpStatusCode.OK;
            response.data = userinvites.Select(x => new OrganizationInviteDTO()
            {
                CreatedAt = x.CreatedAt,
                Id = x.Id,
                OrgId = x.OrgId,
                Org = x.Org.Name,
                Status = x.Status
            }).ToList();
            return response;
        }

        public async Task<Response<bool>> ToggleInvite(int userId, ToggleInviteDTO model)
        {
            Response<bool> response = new();
            User? user = await db_context.Users.FindAsync(userId);
            if (user is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            var invite = await db_context.OrganizationInvites.Where(x => x.Id == model.InviteId && x.IsDeleted == false).FirstOrDefaultAsync();
            var organization = await db_context.Organizations.FindAsync(invite?.OrgId);
            if (invite is null || organization is null)
            {
                response.success = false;
                response.message = "Invite/Organization not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            if (invite.Status != null)
            {
                if ((bool)invite.Status)
                {
                    response.success = true;
                    response.message = "Invite Already Accepted";
                    response.statusCode = HttpStatusCode.OK;
                    response.data = true;
                    return response;
                }
            }

            invite.Status = model.Status;
            if (model.Status)
            {
                user.Org = organization;
                user.OrgId = organization.Id;
            }

            await db_context.SaveChangesAsync();
            response.message = "Successful Operation";
            response.statusCode = HttpStatusCode.OK;
            response.data = true;
            return response;
        }

        // update a user
        public async Task<Response<UserDTO>> UpdateUserProfile(
            int userId, UpdateUserDTO model)
        {
            Response<UserDTO> response = new();
            if (model.Photo != null)
            {
                model.ProfilePic = null;
                var uploadImage = await UploadPhoto(model.Photo, userId);
                if (!uploadImage.success)
                {
                    response.success = false;
                    response.message = uploadImage.message;
                    response.statusCode = uploadImage.statusCode;
                    return response;
                }
            }
            User? user = await db_context.Users.FindAsync(userId);
            if (model.ProfilePic == string.Empty || model.ProfilePic == "string")
            {
                model.ProfilePic = null;
            }
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

                db_context.Entry(user).State = EntityState.Modified;
                user.LastName = model.LastName ?? user.LastName;
                user.FirstName = model.FirstName ?? user.FirstName;
                user.Phone = model.Phone ?? user.Phone;
                user.ProfilePic = model.ProfilePic ?? user.ProfilePic;

                var userDTO =  new UserDTO()
                {
                    Name = user.LastName + " " + user.FirstName,
                    Email = user.Email,
                    Profile_Picture = user.ProfilePic,
                    Role = (bool)user.IsAdmin ? "Admin" : "User",
                    UserId = user.Id.ToString(),
                     
                };
                await db_context.SaveChangesAsync();

                return new Response<UserDTO>
                {
                    success = true,
                    data = userDTO,
                    message = "User Profile updated successfully",
                    statusCode = HttpStatusCode.OK,
                };
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

        public async Task<Response<UserReadAllDTO>> GetAllUsersByOrganization(int user_id)
        {
            List<UserReadDTO> org_people = new();
            List<UserReadDTO> other_people = new();

            try
            {
                // ensure user exists
                if (!await db_context.Users.AnyAsync(x => x.Id == user_id))
                {
                    return new Response<UserReadAllDTO>()
                    {
                        message = "User not found",
                        success = false,
                        statusCode = HttpStatusCode.NotFound
                    };
                }

                long? org_id = (await db_context.Users.FirstOrDefaultAsync(x => x.Id == user_id))?.OrgId;

                if (org_id == null)
                {
                    // do nothing, the list is already empty
                }

                org_people = await db_context.Users
                                    .Where(x => x.OrgId == org_id)
                                    .Select(x => new UserReadDTO(
                                        $"{x.FirstName} {x.LastName}",
                                        x.Email,
                                        x.ProfilePic,
                                        x.Id.ToString(),
                                        x.IsAdmin == null ? "User" : (bool)x.IsAdmin ? "Admin" : "User"
                                    )).ToListAsync();

                // remove the current user from the list
                org_people.RemoveAll(users => users.user_id == user_id.ToString());

                // get everyone else with a non-null org
                other_people = await db_context.Users
                                    .Where(x => x.OrgId != null && x.OrgId != org_id)
                                    .Select(x => new UserReadDTO(
                                        $"{x.FirstName} {x.LastName}",
                                        x.Email,
                                        x.ProfilePic,
                                        x.Id.ToString(),
                                        x.IsAdmin == null ? "User" : (bool)x.IsAdmin ? "Admin" : "User"
                                    )).ToListAsync();

                var res = new UserReadAllDTO()
                {
                    org = org_people,
                    others = other_people
                };

                return new Response<UserReadAllDTO>()
                {
                    data = res,
                    message = "Users fetched successfully"
                };
            }
            catch (Exception)
            {
                return new Response<UserReadAllDTO>()
                {
                    message = "Internal Server Error",
                    statusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response<UserProfileReadDTO>> GetUserProfile(int id)
        {
            try
            {
                var user = await db_context.Users.Include(x => x.Org).FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new Response<UserProfileReadDTO>()
                    {
                        message = "User not found",
                        success = false,
                        statusCode = HttpStatusCode.NotFound
                    };
                }

                var userprofile = new UserProfileReadDTO
                (
                    user_id: user.Id.ToString(),
                    name: $"{user.FirstName} {user.LastName}",
                    email: user.Email,
                    profile_picture: user.ProfilePic,
                    phone_number: user.Phone,
                    isAdmin: user.IsAdmin ?? false,
                    organization: user.Org?.Name ?? "Unassigned",
                    balance: user.LunchCreditBalance.ToString() ?? "0"
                );

                return new Response<UserProfileReadDTO>()
                {
                    data = userprofile,
                    message = "User data fetched successfully"
                };
            }
            catch (Exception)
            {
                return new Response<UserProfileReadDTO>()
                {
                    message = "Internal Server Error",
                    statusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response<UserBankUpdateDTO>> UpdateUserBank(UserBankUpdateDTO userbank, int user_id)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Id == user_id);
                if (user == null)
                {
                    return new Response<UserBankUpdateDTO>()
                    {
                        message = "User not found",
                        success = false,
                        statusCode = HttpStatusCode.NotFound
                    };
                }
                user.BankRegion = userbank.bank_region;
                user.BankNumber = userbank.bank_number;
                user.BankName = userbank.bank_name;
                user.BankCode = userbank.bank_code;
                await db_context.SaveChangesAsync();

                return new Response<UserBankUpdateDTO>()
                {
                    data = null,
                    message = "Successfully created bank account"
                };
            }
            catch (Exception)
            {
                return new Response<UserBankUpdateDTO>()
                {
                    message = "Internal Server Error",
                    statusCode = HttpStatusCode.InternalServerError
                };
            }

        }

        public async Task<Response<UserReadDTO>> SearchForUser(string email)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new Response<UserReadDTO>()
                    {
                        message = "User not found",
                        success = false,
                        statusCode = HttpStatusCode.NotFound
                    };
                }

                var userReadDto = new UserReadDTO(
                    name: $"{user.FirstName} {user.LastName}",
                    email: user.Email,
                    profile_picture: user.ProfilePic,
                    user_id: user.Id.ToString(),
                    role: (bool)user.IsAdmin ? "Admin" : "User"
                    );

                return new Response<UserReadDTO>()
                {
                    data = userReadDto,
                    message = "User found"
                };
            }
            catch (Exception)
            {
                return new Response<UserReadDTO>()
                {
                    message = "Internal Server Error",
                    statusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response<UserReadDTO>> ForgotUserPassword(string email)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new Response<UserReadDTO>()
                    {
                        message = "User not found",
                        success = false,
                        statusCode = HttpStatusCode.NotFound
                    };
                }
                Random rnd = new();
                int reset_token = rnd.Next(1000, 9999);
                user.ResetToken = reset_token.ToString();
                await db_context.SaveChangesAsync();
                _emailService.SendEmail(new MailData
                {
                    EmailBody = reset_token.ToString(),
                    EmailSubject = "Refresh Token",
                    EmailToName = $"{user.FirstName}",
                    EmailToId = email
                });
                return new Response<UserReadDTO>()
                {
                    message = "Link sent to email address",
                    success = false,
                    statusCode = HttpStatusCode.OK
                };
            }
            catch (Exception)
            {
                return new Response<UserReadDTO>()
                {
                    message = "Internal Server Error",
                    success = false,
                    statusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response<UserReadDTO>> ResetUserPassword(ResetPasswordDTO resetDto)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Email == resetDto.Email);
                if (user == null)
                {
                    return new Response<UserReadDTO>()
                    {
                        message = "User not found",
                        success = false,
                        statusCode = HttpStatusCode.NotFound
                    };
                }

                if (resetDto.ResetToken != user.ResetToken)
                {
                    return new Response<UserReadDTO>()
                    {
                        message = "Token is invalid",
                        success = false,
                        statusCode = HttpStatusCode.BadRequest
                    };
                }

                if (string.IsNullOrWhiteSpace(resetDto.NewPassword))
                {
                    return new Response<UserReadDTO>()
                    {
                        message = "Password is invalid",
                        success = false,
                        statusCode = HttpStatusCode.BadRequest
                    };
                }

                authentication.CreatePasswordHash(resetDto.NewPassword, out string password_hash);
                user.PasswordHash = password_hash;
                await db_context.SaveChangesAsync();
                return new Response<UserReadDTO>()
                {
                    message = "Password changed successfully",
                    success = true,
                    statusCode = HttpStatusCode.OK
                };
            }
            catch (Exception)
            {
                return new Response<UserReadDTO>()
                {
                    message = "Internal Server Error",
                    success = false,
                    statusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response<Dictionary<string, string>>> ChangePassword(ChangePasswordDTO model)
        {
            Response<Dictionary<string, string>> response = new();
            User? user_exists = await db_context.Users.Where(u => u.Email == model.Email).Include(x => x.Org).FirstOrDefaultAsync();

            var userindb = mapper.Map<CreateUserDTO>(user_exists);

            // ensure user exists
            if (user_exists is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.Unauthorized;
                response.data = new Dictionary<string, string>() {
                    { "email", model.Email }
                };

                return response;
            }


            // ensure password is correct
            try
            {
                var passwordIsValid = authentication.verifyPasswordHash(model.OldPassword, user_exists.PasswordHash);

                if (!passwordIsValid)
                {
                    response.success = false;
                    response.message = "Incorrect password";
                    response.statusCode = HttpStatusCode.Unauthorized;
                }
                else
                {
                    try
                    {
                        authentication.CreatePasswordHash(model.NewPassword, out string password_hash);

                        user_exists.PasswordHash = password_hash;

                        db_context.Users.Update(user_exists);
                        await db_context.SaveChangesAsync();


                        Dictionary<string, string> data = new();

                        data.Add("FirstName", user_exists.FirstName!);
                        data.Add("LastName", user_exists.LastName!);
                        data.Add("Organization", user_exists.Org?.Name ?? "");

                        response.success = true;
                        response.message = "User successfuly changed password";
                        response.statusCode = HttpStatusCode.OK;
                        response.data = data;
                    }

                    // catch any errors
                    catch (Exception ex)
                    {
                        response.statusCode = HttpStatusCode.InternalServerError;
                        response.success = false;
                        response.message = ex.Message;
                    }

                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                response.statusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<Response<UserReadDTO>> VerifyResetToken(string email, string code)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new Response<UserReadDTO>()
                    {
                        message = "User not found",
                        success = false,
                        statusCode = HttpStatusCode.NotFound
                    };
                }
                bool equal = user.ResetToken == code;
                return new Response<UserReadDTO>()
                {
                    message = equal ? "Token is valid" : "Token is invalid",
                    success = equal,
                    statusCode = equal ? HttpStatusCode.OK : HttpStatusCode.BadRequest
                };
            }
            catch (Exception)
            {
                return new Response<UserReadDTO>()
                {
                    message = "Internal Server Error",
                    success = false,
                    statusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    
        public async Task<Response<bool>> SendInvitationRequest(int userId, int orgId)
        {
            Response<bool> response = new();
            User? user = await db_context.Users.FindAsync(userId);
           
            if (user is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            var sentInviteRequest = db_context.InvitationRequests.Where(x => x.UserEmail == user.Email).ToList();
            if(sentInviteRequest.Any())
            {
                var requested = sentInviteRequest.FirstOrDefault(x => x.OrgId == orgId);
                if(requested != null)
                {
                    response.success = false;
                    response.message = "You already sent this organization an invite";
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.data = false;
                    return response;
                }
            }
            if (user.OrgId != null)
            {
                response.success = false;
                response.message = "You already belong to an organization";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            var organization = await db_context.Organizations.FindAsync(orgId);
            if (organization is null)
            {
                response.success = false;
                response.message = "Invite/Organization not found";
                response.statusCode = HttpStatusCode.NotFound;
                response.data = false;
                return response;
            }
            var inviteRequest = new InvitationRequest()
            {
                OrgId = orgId,
                UserEmail = user.Email,
                Status = false
            };
            await db_context.AddAsync(inviteRequest);
            await db_context.SaveChangesAsync();
            response.message = "Successful Operation";
            response.statusCode = HttpStatusCode.OK;
            response.data = true;
            return response;
        }
    }
}