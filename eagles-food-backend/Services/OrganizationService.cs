using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly LunchDbContext _context;
        private readonly IResponseService _responseService;
        public OrganizationService(LunchDbContext context, IResponseService responseService) {
            _context = context;
            _responseService = responseService;
        }

        public async Task<ServiceResponse<string>> CreateOrganization(CreateOrganizationDTO model)
        {
            if (model == default) return _responseService.ErrorResponse<string>("Something went Wrong");
            var nameExist = await _context.Organizations.AnyAsync(x=>x.name.ToLower() == model.Name.ToLower());
            if(nameExist) return _responseService.ErrorResponse<string>("Name Already Exist");
            if(model.LunchPrice <= 0) return _responseService.ErrorResponse<string>("Cost must be greater than zero");
            var organization = new Organization()
            {
                 name = model.Name,
                 currency = model.Currency,
                 lunch_price = model.LunchPrice
            };
            await _context.AddAsync(organization);
            var saved = await _context.SaveChangesAsync() > 0;
            if(saved)return _responseService.SuccessResponse("Organization Created Successfully", "");
            return _responseService.ErrorResponse<string>("Unable to Create Organization");
        }

        public async Task<ServiceResponse<OrganizationDTO>> GetOrganization(int id){
            var organization = await _context.Organizations.FindAsync(id);
            if(organization == null) return _responseService.ErrorResponse<OrganizationDTO>("Organization not found");
            var response = new OrganizationDTO()
            {
                Currency = organization.currency,
                LunchPrice = organization.lunch_price,
                Name = organization.name,
            };
            return _responseService.SuccessResponse(response);
        }
    }

    public interface IOrganizationService
    {
        Task<ServiceResponse<string>> CreateOrganization(CreateOrganizationDTO model);
        Task<ServiceResponse<OrganizationDTO>> GetOrganization(int id);
    }
}
