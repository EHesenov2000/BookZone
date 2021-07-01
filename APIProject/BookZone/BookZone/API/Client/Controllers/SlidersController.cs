using AutoMapper;
using BookZone.API.Client.DTOs.SliderDtos;
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
    public class SlidersController : ControllerBase
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public SlidersController(ISliderRepository sliderRepository,IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Slider> sliders = await _sliderRepository.GetAllAsync();
            List<SliderGetDto> getSliders = _mapper.Map<List<SliderGetDto>>(sliders);
            //    new List<SliderGetDto>();
            //foreach (var item in sliders)
            //{
            //    SliderGetDto getDto = new SliderGetDto
            //    {
            //        Id = item.Id,
            //        Image = item.Image
            //    };
            //    getSliders.Add(getDto);
            //}
           return StatusCode(200,getSliders);
        }
    }
}
