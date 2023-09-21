namespace eagles_food_backend.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly LunchDbContext _context;
        private readonly IResponseService _responseService;
        public OrganizationService(LunchDbContext context, IResponseService responseService)
        {
            _context = context;
            _responseService = responseService;
        }

        public async Task<ServiceResponse<CreateOrganizationDTO>> CreateOrganization(CreateOrganizationDTO model)
        {
            if (model == default) return _responseService.ErrorResponse<CreateOrganizationDTO>("Something went Wrong");
            var nameExist = await _context.Organizations.AnyAsync(x => x.Name.ToLower() == model.Name.ToLower());
            if (nameExist) return _responseService.ErrorResponse<CreateOrganizationDTO>("Name Already Exist");
            if (model.LunchPrice <= 0) return _responseService.ErrorResponse<CreateOrganizationDTO>("Cost must be greater than zero");
            var organization = new Organization()
            {
                Name = model.Name,
                CurrencyCode = model.Currency,
                LunchPrice = (decimal)model.LunchPrice
            };
            await _context.AddAsync(organization);
            var saved = await _context.SaveChangesAsync() > 0;
            if (saved) return _responseService.SuccessResponse(model);
            return _responseService.ErrorResponse<CreateOrganizationDTO>("Unable to Create Organization");
        }

        public async Task<ServiceResponse<OrganizationDTO>> GetOrganization(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null) return _responseService.ErrorResponse<OrganizationDTO>("Organization not found");
            var response = new OrganizationDTO()
            {
                Currency = organization.CurrencyCode,
                LunchPrice = organization.LunchPrice,
                Name = organization.Name,
            };
            return _responseService.SuccessResponse(response);
        }
    }
}
