using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.OrganizationRepository
{
    public interface IOrganizationService
    {
        // Task<ServiceResponse<OrganizationDTO>> GetOrganization(int id);
        Task<Response<Dictionary<string, string>>> CreateStaffMember(CreateStaffDTO model);
        Task<Response<Dictionary<string, string>>> ModifyOrganization(int UserID, ModifyOrganizationDTO model);
    }
}