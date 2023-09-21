namespace eagles_food_backend.Domains
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDTO, User>();
            //CreateMap<User, CreateUserDTO>();
        }
    }
}
