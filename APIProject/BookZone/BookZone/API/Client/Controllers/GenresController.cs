using AutoMapper;
using BookZone.API.Client.DTOs.GenreDtos;
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
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenresController(IGenreRepository genreRepository,IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }
        [HttpGet("getall/{categoryId}")]
        public async Task<IActionResult> GetAllByCategory(int categoryId)
        {
            List<Genre> genres = await _genreRepository.GetAllAsync(categoryId);
            List<GenreGetDto> getDtos = _mapper.Map<List<GenreGetDto>>(genres);
            //    new List<GenreGetDto>();
            //foreach (var item in genres)
            //{
            //    GenreGetDto genre = new GenreGetDto
            //    {
            //        Id = item.Id,
            //        Name = item.Name
            //    };
            //    getDtos.Add(genre);
            //}
            return StatusCode(200, getDtos);
        }
    }
}
