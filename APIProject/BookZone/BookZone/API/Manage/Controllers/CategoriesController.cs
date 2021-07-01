using BookZone.API.Manage.DTOs.CategoryDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(IWebHostEnvironment env, ICategoryRepository categoryRepository)
        {
            _env = env;
            _categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryDto)
        {
            if (categoryDto == null) return Conflict();
            if (categoryDto.Name == null) return BadRequest();
            Category category = new Category
            {
                Name = categoryDto.Name,
            };
            if (await _categoryRepository.IsExist(category)) return BadRequest();
            await _categoryRepository.CreateAsync(category);
            await _categoryRepository.CommitAsync();

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int count = 10)
        {
            if (await _categoryRepository.GetAllAsync(page, count) == null) return NotFound();
            List<Category> categories = await _categoryRepository.GetAllAsync(page, count);
            List<CategoryGetDto> getCategories = new List<CategoryGetDto>();
            foreach (var item in categories)
            {
                CategoryGetDto category = new CategoryGetDto
                {
                    Id = item.Id,
                    Name = item.Name,
                };
                getCategories.Add(category);
            }
            return StatusCode(200, getCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category categoryeDatabase = await _categoryRepository.GetAsync(id);
            if (categoryeDatabase == null) return NotFound();
            CategoryGetDto category = new CategoryGetDto
            {
                Id = categoryeDatabase.Id,
                Name = categoryeDatabase.Name,

            };
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _categoryRepository.GetAsync(id) == null) return NotFound();
            Category category = await _categoryRepository.GetAsync(id);
            if (category.Genres.Count !=0) return Conflict();
            _categoryRepository.Remove(category);
            await _categoryRepository.CommitAsync();
            return Ok();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(CategoryCreateDto updateDto, int id)
        {
            Category existCategory = await _categoryRepository.GetAsync(id);
            if (existCategory == null) return NotFound();
            if (updateDto == null || updateDto.Name == null) return BadRequest();
            Category category = new Category
            {
                Id=id,
                Name = updateDto.Name,
            };
            if (await _categoryRepository.IsExist(category)) return BadRequest();

            existCategory.Name = updateDto.Name;

            await _categoryRepository.CommitAsync();
            return Ok();

        }
    }
}
