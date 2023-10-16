using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Filters;
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
        Task<Response<List<OrganizationInvitationDTO>>> OrganizationInvites(int userId, PaginationFilter validFilter);
        Task<Response<string>> HideOrganization(int userId, bool hide);
        Task<Response<OrganizationDTO>> GetOrganization(int userId);
        Task<Response<List<OrganizationReadDTO>>> GetAllOrganizations(PaginationFilter validFilter);
        Task<Response<List<OrganizationInvitationDTO>>> OrganizationInviteRequests(int userId, PaginationFilter validFilter);
        Task<Response<bool>> ToggleInviteRequest(int userId, ToggleInviteDTO model);
    }
}