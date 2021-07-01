using AutoMapper;
using BookZone.API.Client.DTOs.CategoryDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            List<Category> categories = await _categoryRepository.GetAllAsync();
            List<CategoryGetDto> getDtos = _mapper.Map<List<CategoryGetDto>>(categories);
                
            //    new List<CategoryGetDto>();
            //foreach (var item in categories)
            //{
            //    CategoryGetDto category = new CategoryGetDto
            //    {
            //        Id = item.Id,
            //        Name = item.Name
            //    };
            //    getDtos.Add(category);
            //}
            return StatusCode(200, getDtos);
        }
    }
}
