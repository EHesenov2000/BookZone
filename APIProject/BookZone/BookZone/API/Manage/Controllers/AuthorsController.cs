using BookZone.API.Manage.DTOs.AuthorDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IWebHostEnvironment env, IAuthorRepository authorRepository)
        {
            _env = env;
            _authorRepository = authorRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateDto authorDto)
        {
            if (authorDto == null) return Conflict();
            if (authorDto.FullName == null) return BadRequest();
            Author author = new Author
            {
                FullName = authorDto.FullName,
                BornYear = authorDto.BornYear,
                IsDeleted = false,
                CreatedAt= DateTime.UtcNow.AddHours(4)
            };
            if (await _authorRepository.IsExist(author)) return Conflict();
            await _authorRepository.CreateAsync(author);
            await _authorRepository.CommitAsync();

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int count = 10)
        {
            if (await _authorRepository.GetAllAsync(page, count) == null) return NotFound();
            List<Author> authors = await _authorRepository.GetAllAsync(page, count);
            List<AuthorItemDto> getAuthors= new List<AuthorItemDto>();
            foreach (var item in authors)
            {
                AuthorItemDto author = new AuthorItemDto
                {
                    Id = item.Id,
                    FullName = item.FullName,
                    BornYear=item.BornYear,
                    CreatedAt=item.CreatedAt,
                };
                getAuthors.Add(author);
            }
            return StatusCode(200, getAuthors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Author authorDatabase = await _authorRepository.GetAsync(id);
            if (authorDatabase == null) return NotFound();
            AuthorItemDto author = new AuthorItemDto
            {
                Id = authorDatabase.Id,
                FullName = authorDatabase.FullName,
                BornYear=authorDatabase.BornYear,
                CreatedAt=authorDatabase.CreatedAt,
            };
            return Ok(author);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _authorRepository.GetAsync(id) == null) return NotFound();
            Author author = await _authorRepository.GetAsync(id);
            author.IsDeleted = true;
            await _authorRepository.CommitAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(AuthorCreateDto updateDto, int id)
        {
            if (updateDto == null || updateDto.FullName == null) return BadRequest();
            Author existAuthor = await _authorRepository.GetAsync(id);
            if (existAuthor == null) return NotFound();
            Author author = new Author
            {
                Id = id,
                FullName = updateDto.FullName,
                BornYear = updateDto.BornYear,
                IsDeleted=existAuthor.IsDeleted,
                CreatedAt=existAuthor.CreatedAt
            };
            if (await _authorRepository.IsExist(author)) return Conflict();
            existAuthor.FullName = updateDto.FullName;
            existAuthor.BornYear = updateDto.BornYear;
            existAuthor.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _authorRepository.CommitAsync();
            return Ok();

        }
    }
}
