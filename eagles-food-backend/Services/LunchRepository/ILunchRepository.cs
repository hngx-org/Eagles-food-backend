﻿using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.LunchRepository
{
    public interface ILunchRepository
    {
        public Task<Response<ResponseLunchDTO>> create(CreateLunchDTO createLunchDTO);
        public Task<Response<List<ResponseLunchDTO>>> getAll();
    }
}
