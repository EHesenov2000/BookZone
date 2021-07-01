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
    public class TagsController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        private readonly ITagRepository _tagRepository;

        public TagsController(IWebHostEnvironment env, ITagRepository tagRepository)
        {
            _env = env;
            _tagRepository = tagRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagCreateDto tagDto)
        {
            if (tagDto.Name == null) return BadRequest();
            Tag tag = new Tag
            {
                Name = tagDto.Name,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                IsDeleted = false

            };
            if (await _tagRepository.IsExist(tag)) return BadRequest();
            await _tagRepository.CreateAsync(tag);
            await _tagRepository.CommitAsync();

            return StatusCode(201);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page=1, int count=10)
        {
            if (await _tagRepository.GetAllAsync(page, count) == null) return NotFound();
            List<Tag> tags = await _tagRepository.GetAllAsync(page, count);
            List<TagItemDto> getTags = new List<TagItemDto>();
            foreach (var item in tags)
            {
                TagItemDto tag = new TagItemDto
                {
                    Id=item.Id,
                    Name=item.Name,
                    CreatedAT=item.CreatedAt,
                    IsDeleted=item.IsDeleted
                };
                getTags.Add(tag);
            }
            return StatusCode(200, getTags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Tag tagDatabase = await _tagRepository.GetAsync(id);
            if (tagDatabase == null) return NotFound();
            TagItemDto tag = new TagItemDto
            {
                Id = tagDatabase.Id,
                Name=tagDatabase.Name,
                CreatedAT=tagDatabase.CreatedAt,
                IsDeleted=tagDatabase.IsDeleted
            };
            return Ok(tag);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _tagRepository.GetAsync(id) == null) return NotFound();
            Tag tag = await _tagRepository.GetAsync(id);
            tag.IsDeleted = true;
            await _tagRepository.CommitAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update( TagUpdateDto updateDto, int id)
        {
            if (updateDto == null || updateDto.Name == null) return BadRequest();
            Tag existag = await _tagRepository.GetAsync(id);
            if (existag == null) return NotFound();

            Tag tag = new Tag
            {
                Id=id,
                Name = updateDto.Name,

            };

            if (await _tagRepository.IsExist(tag)) return BadRequest();


            existag.Name = updateDto.Name;
            existag.CreatedAt = DateTime.UtcNow.AddHours(4);
            existag.IsDeleted = updateDto.IsDeleted;

            await _tagRepository.CommitAsync();
            return Ok();

        }
    }
}
