using AutoMapper;
using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Services.ResponseService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace eagles_food_backend.Services.LunchRepository
{
    public class LunchService : ILunchRepository
    {
        private readonly LunchDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LunchService(LunchDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

                #region Check for Invalid Request
                if (!createLunchDTO.receivers.Any())
                {
                    response.message = "Invalid request.";
                    response.success = false;
                    response.statusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                createLunchDTO.receivers = createLunchDTO.receivers.Distinct().ToArray();
                if (createLunchDTO.receivers.Contains(senderId))
                {
                    response.message = "Sender cannot be among the receivers. Invalid request.";
                    response.success = false;
                    response.statusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                #endregion
                #region check receiver id
                var nonExistentUserIds = createLunchDTO.receivers
                    .Where(id => !_context.Users.Any(x => x.Id == id))
                    .ToList();

                if (nonExistentUserIds.Any())
                {
                    response.message = "Users with the following IDs do not exist: " + string.Join(", ", nonExistentUserIds);
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
                var lunchList = createLunchDTO.receivers.Select(id => new Lunch
                {
                    ReceiverId = id,
                    SenderId = senderId,
                    OrgId = sender.OrgId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    Redeemed = false,
                    Quantity = createLunchDTO.quantity,
                    Note = createLunchDTO.note,
                    Receiver = _context.Users.FirstOrDefault(x => x.Id == id)
                }).ToList();
                foreach (var lun in lunchList)
                {
                    if (lun.Receiver != null)
                        lun.Receiver.LunchCreditBalance = lun.Receiver.LunchCreditBalance + lun.Quantity;
                }
                _context.Lunches.AddRange(lunchList);
                await _context.SaveChangesAsync();

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
                     Quantity = x.Quantity,
                     Redeemed = x.Redeemed ?? false
                 })
                 .ToListAsync();

                response.data = newList;
                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Error getting all lunches";
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
            response.data = _mapper.Map<ResponseLunchDTO>(result);
            response.success = true;
            response.statusCode = HttpStatusCode.OK;
            return response;
        }

    }
}
