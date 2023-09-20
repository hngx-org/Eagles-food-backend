using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services
{

    public interface ILunchService
    {
        public Task<Lunch> create(CreateLunchDTO createLunchDTO);
        public Task<List<Lunch>> getAll(CreateLunchDTO createLunchDTO);
    }
    public class LunchService : ILunchService
    {
        private readonly LunchDbContext _context;
        private readonly IResponseService _responseService;
        public LunchService(LunchDbContext context, IResponseService responseService)
        {
            _context = context;
            _responseService = responseService;
        }
        public async Task<Lunch> create(CreateLunchDTO createLunchDTO)
        {
            var checkSenderExist = await _context.Users.Any(x => x.UserId == createLunchDTO.senderId);

            #region save lunch
            var lunch = new Lunch()
            {
                created_at = DateTime.UtcNow,
                note = createLunchDTO.note,
                quantity = createLunchDTO.quantity,
                recieverId = createLunchDTO.receiverId,
                senderId = createLunchDTO.senderId,
                redeemed = false
            };
            _context.Lunches.Add(lunch);
            await _context.SaveChangesAsync();
            #endregion 
            return lunch;
        }

        public Task<List<Lunch>> getAll(CreateLunchDTO createLunchDTO)
        {
            throw new NotImplementedException();
        }
    }
}
