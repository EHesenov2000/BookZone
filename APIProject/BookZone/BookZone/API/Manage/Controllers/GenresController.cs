using BookZone.API.Manage.DTOs.GenreDtos;
using BookZone.API.Manage.DTOs.TagDtos;
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
    public class GenresController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IGenreRepository _genreRepository;
        private readonly ICategoryRepository _categoryRepository;

        public GenresController(IWebHostEnvironment env, IGenreRepository genreRepository,ICategoryRepository categoryRepository)
        {
            _env = env;
            _genreRepository = genreRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenreCreateDto genreDto)
        {
            if (genreDto == null) return Conflict();
            if (genreDto.Name == null || genreDto.CategoryId==0) return BadRequest();
            if (await _categoryRepository.GetAsync(genreDto.CategoryId) == null) return Conflict();
            Genre genre = new Genre
            {
                Name = genreDto.Name,
                CategoryId=genreDto.CategoryId
            };
            if (await _genreRepository.IsExist(genre)) return BadRequest();

            await _genreRepository.CreateAsync(genre);
            await _genreRepository.CommitAsync();

            return StatusCode(201);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int count = 10)
        {
            if (await _genreRepository.GetAllAsync(page, count) == null) return NotFound();
            List<Genre> genres = await _genreRepository.GetAllAsync(page, count);
            if (genres == null) return NotFound();
            List<GenreGetDto> getGenres = new List<GenreGetDto>();
            foreach (var item in genres)
            {
                GenreGetDto genre = new GenreGetDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    CategoryName=item.Category.Name
                };
                getGenres.Add(genre);
            }
            return StatusCode(200, getGenres);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Genre genreDatabase = await _genreRepository.GetAsync(id);
            if (genreDatabase == null) return NotFound();
            GenreGetDto genre = new GenreGetDto
            {
                Id = genreDatabase.Id,
                Name = genreDatabase.Name,
                CategoryName=genreDatabase.Category.Name

            };
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _genreRepository.GetAsync(id) == null) return NotFound();
            Genre genre = await _genreRepository.GetAsync(id);
            if (genre.Books.Count != 0) return Conflict();
            _genreRepository.Remove(genre);
            await _genreRepository.CommitAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(GenreCreateDto updateDto, int id)
        {
            if (updateDto == null) return BadRequest();
            if (await _categoryRepository.GetAsync(updateDto.CategoryId) == null) return Conflict();

            Genre existGenre = await _genreRepository.GetAsync(id);
            if (existGenre == null) return NotFound();
            Genre newGenre = new Genre
            {
                Id = id,
                Name = updateDto.Name,
                CategoryId=updateDto.CategoryId
            };
            if( await _genreRepository.IsExist(newGenre))return BadRequest();
            existGenre.Name = updateDto.Name;
            existGenre.CategoryId = updateDto.CategoryId;


            await _genreRepository.CommitAsync();
            return Ok();

        }
    }
}
