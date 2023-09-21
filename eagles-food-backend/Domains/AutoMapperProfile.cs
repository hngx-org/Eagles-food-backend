﻿using AutoMapper;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Domains
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<CreateLunchDTO, Lunch>();
            CreateMap<ResponseLunchDTO, Lunch>();
        }
    }
}
