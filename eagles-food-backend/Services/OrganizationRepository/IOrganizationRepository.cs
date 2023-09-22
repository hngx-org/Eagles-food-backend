using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.OrganizationRepository
{
    public interface IOrganizationService
    {
        Task<Response<Dictionary<string, string>>> CreateStaffMember(CreateStaffDTO model);
        Task<Response<Dictionary<string, string>>> ModifyOrganization(int UserID, ModifyOrganizationDTO model);
        Task<Response<Dictionary<string, string>>> UpdateOrganizationWallet(int UserID, UpdateOrganizationWalletDTO model);
        Task<Response<Dictionary<string, string>>> UpdateOrganizationLunchPrice(int UserID, UpdateOrganizationLunchPriceDTO model);
        Task<Response<Dictionary<string, string>>> InviteToOrganization(int UserID, InviteToOrganizationDTO model);
    }
}