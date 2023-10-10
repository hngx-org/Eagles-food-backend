using System.Net;

using CloudinaryDotNet;

using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Services.EmailService;

using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services.LunchRepository
{
    public class LunchService : ILunchRepository
    {
        private readonly LunchDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public LunchService(LunchDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;

        }
        public async Task<Response<ResponseLunchDTO>> create(CreateLunchDTO createLunchDTO)
        {
            Response<ResponseLunchDTO> response = new Response<ResponseLunchDTO>();
            try
            {
                var id = _httpContextAccessor.HttpContext.User.Identity.Name;
                if (id == null)
                {
                    response.message = $"UnAuthorized";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    return response;
                }
                int senderId = int.Parse(id);
                string? senderEmail = _context.Users.FirstOrDefault(x => x.Id == senderId)?.Email;
                #region Check for Invalid Request
                if (!createLunchDTO.receivers.Any())
                {
                    response.message = "Invalid request.";
                    response.success = false;
                    response.statusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                createLunchDTO.receivers = createLunchDTO.receivers.Distinct().ToArray();
                if (createLunchDTO.receivers.Contains(senderEmail))
                {
                    response.message = "Sender cannot be among the receivers. Invalid request.";
                    response.success = false;
                    response.statusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                #endregion
                #region check receiver id
                var nonExistentUserEmails = createLunchDTO.receivers
                    .Where(email => !_context.Users.Any(x => x.Email == email))
                    .ToList();

                if (nonExistentUserEmails.Any())
                {
                    response.message = "Users with the following emails do not exist: " + string.Join(", ", nonExistentUserEmails);
                    response.success = false;
                    response.statusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                #endregion
                #region check sender id
                var sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == senderId);
                if (sender == null)
                {
                    response.message = $"User with the ID {senderId} do not exist";
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    return response;
                }
                #endregion
                #region check quantity
                if (createLunchDTO.quantity < 1)
                {
                    response.message = "Invalid quantity";
                    response.success = false;
                    return response;
                }
                #endregion

                #region check quantity
                if (createLunchDTO.quantity < 1)
                {
                    response.message = "Invalid quantity";
                    response.success = false;
                    return response;
                }
                #endregion

                #region save lunch
                var lunchList = createLunchDTO.receivers.Select(email => new Lunch
                {
                    ReceiverId = _context.Users.FirstOrDefault(x => x.Email == email).Id,
                    SenderId = senderId,
                    OrgId = sender.OrgId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    Redeemed = false,
                    Quantity = createLunchDTO.quantity,
                    LunchStatus = LunchStatus.Sending,
                    Note = createLunchDTO.note,
                    Receiver = _context.Users.FirstOrDefault(x => x.Email == email)
                }).ToList();
                foreach (var lun in lunchList)
                {
                    if (lun.Receiver != null)
                        lun.Receiver.LunchCreditBalance = lun.Receiver.LunchCreditBalance + lun.Quantity;
                }
                _context.Lunches.AddRange(lunchList);
                await _context.SaveChangesAsync();

                foreach (var lun in lunchList)
                {
                    _emailService.SendEmail(new MailData()
                    {
                        EmailBody = $"Hi {lun.Receiver.FirstName}, You have received {lun.Quantity} lunch(es)",
                        EmailSubject = "New lunch",
                        EmailToId = lun.Receiver.Email,
                        EmailToName = lun.Receiver.FirstName
                    });
                }
                response.message = "Lunch request created successfully";
                response.data = null;
                response.statusCode = HttpStatusCode.Created;
                response.success = true;
                #endregion
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Error creating lunch";
                return response;
            }
        }

        public async Task<Response<List<ResponseLunchDTO>>> getAll()
        {
            Response<List<ResponseLunchDTO>> response = new Response<List<ResponseLunchDTO>>();
            try
            {
                var id = _httpContextAccessor.HttpContext.User.Identity.Name;
                if (id == null)
                {
                    response.message = $"UnAuthorized";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    return response;
                }
                int userId = int.Parse(id);
                var newList = await _context.Lunches
                 .Where(x => x.ReceiverId == userId || x.SenderId == userId)
                 .Select(x => new ResponseLunchDTO()
                 {
                     Id = x.Id,
                     ReceiverName = _context.Users.FirstOrDefault(y => y.Id == x.ReceiverId).FirstName,
                     SenderName = _context.Users.FirstOrDefault(y => y.Id == x.SenderId).FirstName,
                     SenderId = x.SenderId ?? 0,
                     ReceiverId = x.ReceiverId ?? 0,
                     CreatedAt = x.CreatedAt ?? DateTime.Now,
                     Note = x.Note,
                     LunchStatus = x.LunchStatus,
                     Quantity = x.Quantity,
                     Redeemed = x.Redeemed ?? false
                 })
                 .ToListAsync();

                response.message = "Success";
                response.data = newList;
                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Error getting all lunches";
                response.statusCode = HttpStatusCode.InternalServerError;
                response.success = false;
                return response;
            }
        }


        public async Task<Response<List<ResponseLunchDTO>>> Leaderboard()
        {
            Response<List<ResponseLunchDTO>> response = new Response<List<ResponseLunchDTO>>();
            try
            {
                var id = _httpContextAccessor.HttpContext.User.Identity.Name;
                if (id == null)
                {
                    response.message = $"UnAuthorized";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    return response;
                }
                int userId = int.Parse(id);
                var newList = await _context.Lunches
                 .Where(x => x.ReceiverId == userId || x.SenderId == userId)
                 .Select(x => new ResponseLunchDTO()
                 {
                     Id = x.Id,
                     ReceiverName = _context.Users.FirstOrDefault(y => y.Id == x.ReceiverId).FirstName,
                     SenderName = _context.Users.FirstOrDefault(y => y.Id == x.SenderId).FirstName,
                     SenderId = x.SenderId ?? 0,
                     ReceiverId = x.ReceiverId ?? 0,
                     CreatedAt = x.CreatedAt ?? DateTime.Now,
                     Note = x.Note,
                     LunchStatus = x.LunchStatus,
                     Quantity = x.Quantity,
                     Redeemed = x.Redeemed ?? false
                 })
                 .ToListAsync();

                response.message = "Success";
                response.data = newList;
                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Error getting all lunches";
                response.statusCode = HttpStatusCode.InternalServerError;
                response.success = false;
                return response;
            }
        }


        public async Task<Response<ResponseLunchDTO>> getById(int id)
        {
            Response<ResponseLunchDTO> response = new Response<ResponseLunchDTO>();
            var result = await _context.Lunches.FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
            {
                response.success = false;
                response.message = $"Lunch with the ID {id} do not exist";
                response.statusCode = HttpStatusCode.BadRequest;
                return response;
            }
            response.message = "Success";
            response.data = _mapper.Map<ResponseLunchDTO>(result);
            response.success = true;
            response.statusCode = HttpStatusCode.OK;
            return response;
        }

        public async Task<Response<ResponseLunchWithdrawalDTO>> withdrawLunch(WithdrawLunchDTO withdrawDTO)
        {
            Response<ResponseLunchWithdrawalDTO> response = new();
            try
            {
                var id = _httpContextAccessor.HttpContext.User.Identity.Name;
                if (id == null)
                {
                    response.message = $"UnAuthorized";
                    response.statusCode = HttpStatusCode.Unauthorized;
                    response.success = false;
                    return response;
                }

                var userLunchBalance = await _context.Users.Include(m => m.Org).Where(m => m.Id == int.Parse(id)).FirstOrDefaultAsync();

                if (userLunchBalance == null)
                {
                    response.message = "Error communicating with server";
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.success = false;
                    return response;
                }

                bool requestExceedsBalance = withdrawDTO.Quantity > userLunchBalance.LunchCreditBalance;

                if (requestExceedsBalance)
                {
                    response.message = "Amount specified exceeds the number of gifted lunches you have";
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.success = false;
                    return response;
                }

                var withdrawalAmount = userLunchBalance.Org.LunchPrice * withdrawDTO.Quantity;
                var leftCreditBalance = userLunchBalance.LunchCreditBalance - withdrawDTO.Quantity;

                userLunchBalance.LunchCreditBalance = leftCreditBalance;

                var lunchRecord = new Lunch
                {
                    ReceiverId = userLunchBalance.Id,
                    SenderId = userLunchBalance.Id,
                    OrgId = userLunchBalance.OrgId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    Redeemed = true,
                    LunchStatus = LunchStatus.Withdrawal,
                    Quantity = withdrawDTO.Quantity,
                    Note = "Lunch Withdrawal",


                };
                await _context.AddAsync(lunchRecord);
                _context.Users.Update(userLunchBalance);
                await _context.SaveChangesAsync();

                ResponseLunchWithdrawalDTO responseLunchWithdrawal = new()
                {
                    WithdrawalAmount = (decimal)withdrawalAmount
                };

                response.data = responseLunchWithdrawal;
                response.message = "withdrawal successful";
                response.statusCode = HttpStatusCode.OK;
                response.success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "Could not process withdrawal request";
                response.statusCode = HttpStatusCode.InternalServerError;
                return response;
            }



        }

    }
}