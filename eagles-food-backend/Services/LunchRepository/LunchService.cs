using AutoMapper;
using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using eagles_food_backend.Services.ResponseServce;
using Microsoft.EntityFrameworkCore;
using System;

namespace eagles_food_backend.Services.LunchRepository
{
    public class LunchService : ILunchRepository
    {
        private readonly LunchDbContext _context;
        private readonly IResponseService _responseService;
        private readonly IMapper _mapper;
        public LunchService(LunchDbContext context, IResponseService responseService, IMapper mapper)
        {
            _context = context;
            _responseService = responseService;
            _mapper = mapper;
        }
        public async Task<Response<ResponseLunchDTO>> create(CreateLunchDTO createLunchDTO)
        {
            Response<ResponseLunchDTO> response = new Response<ResponseLunchDTO>();
            try
            {
                #region check sender id
                var checkSenderExist = _context.users.Any(x => x.Id == createLunchDTO.senderId);
                if (!checkSenderExist)
                {
                    response.message = "Sender dose not exist";
                    response.success = false;
                    return response;
                }
                #endregion
                #region check receiver id
                var checkReceiverExist = _context.users.Any(x => x.Id == createLunchDTO.receiverId);
                if (!checkReceiverExist)
                {
                    response.message = "Receiver dose not exist";
                    response.success = false;
                    return response;
                }
                #endregion
                #region check organization id
                var checkOrganizationExist = _context.organizations.Any(x => x.Id == createLunchDTO.org_id);
                if (!checkOrganizationExist)
                {
                    response.message = "Organization dose not exist";
                    response.success = false;
                    return response;
                }
                #endregion
                #region save lunch
                var lunch = _mapper.Map<Lunch>(createLunchDTO);
                lunch.created_at = DateTime.UtcNow;
                _context.lunches.Add(lunch);
                await _context.SaveChangesAsync();
                var responeDto = _mapper.Map<ResponseLunchDTO>(lunch);
                response.data = responeDto;
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
                var result = await _context.lunches.ToListAsync();
                var newList = result.Select(x => _mapper.Map<ResponseLunchDTO>(x)).ToList();
                var responeDto = newList;
                response.data = responeDto;
                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Error getting all lunches";
                return response;
            }
        }
    }
}
