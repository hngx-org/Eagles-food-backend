using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.LunchRepository
{
    public interface ILunchRepository
    {
        public Task<Response<ResponseLunchDTO>> create(CreateLunchDTO createLunchDTO);
        public Task<Response<List<ResponseLunchDTO>>> getAll();
        public Task<Response<ResponseLunchDTO>> getById(int id);
        Task<Response<List<LeaderBoardResponseDTO>>> Leaderboard();
        public Task<Response<ResponseLunchWithdrawalDTO>> withdrawLunch(WithdrawLunchDTO withdrawLunchDTO);
        public Task<Response<LunchBalanceDTO>> GetLunchBalance();
    }
}