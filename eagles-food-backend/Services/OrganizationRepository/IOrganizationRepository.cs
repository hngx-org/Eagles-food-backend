using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.OrganizationRepository
{
    public interface IOrganizationService
    {
        Task<ServiceResponse<CreateOrganizationDTO>> CreateOrganization(CreateOrganizationDTO model);
        Task<ServiceResponse<OrganizationDTO>> GetOrganization(int id);
    }
}
