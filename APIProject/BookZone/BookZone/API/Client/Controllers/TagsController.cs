using AutoMapper;
using BookZone.API.Client.DTOs.TagDtos;
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
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagsController(ITagRepository tagRepository,IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            List<Tag> tags = await _tagRepository.GetAllAsync();
            List<TagGetDto> getDtos = _mapper.Map<List<TagGetDto>>(tags);
                //new List<TagGetDto>();

            //foreach (var item in tags)
            //{
            //    TagGetDto tag = new TagGetDto
            //    {
            //        Id=item.Id,
            //        Name = item.Name
            //    };
            //    getDtos.Add(tag);
            //}
            return StatusCode(200, getDtos);
        }
    }
}
