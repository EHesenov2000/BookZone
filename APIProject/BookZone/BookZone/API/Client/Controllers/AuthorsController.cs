using BookZone.API.Client.DTOs.AuthorDtos;
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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            List<Author> authors = await _authorRepository.GetAllAsync();
            List<AuthorGetDto> getDtos = new List<AuthorGetDto>();
            foreach (var item in authors)
            {
                AuthorGetDto author = new AuthorGetDto
                {
                    Id = item.Id,
                    FullName = item.FullName,
                    BornYear = item.BornYear
                };
                getDtos.Add(author);
            }
            return StatusCode(200, getDtos);
        }
    }
}
