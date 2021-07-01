using BookZone.API.Manage.DTOs.SliderDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ISliderRepository _sliderRepository;

        public SlidersController(IWebHostEnvironment env,ISliderRepository sliderRepository)
        {
            _env = env;
            _sliderRepository = sliderRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]SliderCreateDto slider)
        {
            if (slider == null) return BadRequest();

            Slider newslider = new Slider { };
            if (slider.File != null)
            {
                if (slider.File.ContentType != "image/png" && slider.File.ContentType != "image/jpeg")
                {
                    return Conflict();
                }
                if (slider.File.Length > (1024 * 1024) * 5)
                {
                    return Conflict();
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + slider.File.FileName;
                var path = Path.Combine(rootPath, "uploads/slider", fileName);
                using ( System.IO.FileStream stream = new FileStream(path, System.IO.FileMode.Create))
                {
                    slider.File.CopyTo(stream);
                }
                newslider.Image = fileName;
            }

            else
            {
                return BadRequest();
            }

            await _sliderRepository.CreateAsync(newslider);
            await _sliderRepository.CommitAsync();
            return StatusCode(201);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page=1,int count=10)
        {
            if (await _sliderRepository.GetAllAsync(page, count) == null) return NotFound();
            List<Slider> sliders = await _sliderRepository.GetAllAsync(page, count);
            List<SliderGetDto> getSliders = new List<SliderGetDto>();
            foreach (var item in sliders)
            {
                SliderGetDto slider = new SliderGetDto
                {
                    Id = item.Id,
                    Image = item.Image
                };
                getSliders.Add(slider);
            }
            return StatusCode(200, getSliders);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Slider sliderDatabase = await _sliderRepository.GetAsync(id);
            SliderGetDto slider = new SliderGetDto
            {
                Id = sliderDatabase.Id,
                Image = sliderDatabase.Image
            };
            return Ok(slider);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _sliderRepository.GetAsync(id) == null) return NotFound();
             _sliderRepository.Remove(await _sliderRepository.GetAsync(id));
            return Ok(await _sliderRepository.CommitAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm]SliderCreateDto createDto, int id)
        {
            if (createDto == null || createDto.File==null) return Conflict();
            Slider existSlider = await _sliderRepository.GetAsync(id);
            if (existSlider == null) return NotFound();

            if (createDto.File.ContentType != "image/png" && createDto.File.ContentType != "image/jpeg")
            {
                return Conflict();
            }
            if (createDto.File.Length > (1024 * 1024) * 5)
            {
                return Conflict();
            }
            string rootPath = _env.WebRootPath;
            var fileName = Guid.NewGuid().ToString() + createDto.File.FileName;
            var path = Path.Combine(rootPath, "uploads/slider", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createDto.File.CopyTo(stream);
            }
            if (existSlider.Image != null)
            {
                string existPath = Path.Combine(_env.WebRootPath, "uploads/slider", existSlider.Image);
                if (System.IO.File.Exists(existPath))
                {
                    System.IO.File.Delete(existPath);
                }
            }
            existSlider.Image = fileName;

            await _sliderRepository.CommitAsync();
            return StatusCode(200);
                
        }
    }

}
