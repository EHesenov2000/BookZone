using AutoMapper;
using BookZone.API.Client.DTOs.SettingDtos;
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
    public class SettingsController : ControllerBase
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IMapper _mapper;

        public SettingsController(ISettingRepository settingRepository, IMapper mapper)
        {
            _settingRepository = settingRepository;
            _mapper = mapper;
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            Setting setting = await _settingRepository.GetAsync();
            SettingGetDto getDto = _mapper.Map<SettingGetDto>(setting);
            //    new SettingGetDto
            //{
            //    Image = setting.Image,
            //    Location = setting.Location,
            //    Contact = setting.Contact,
            //    Email = setting.Email,
            //    Facebook = setting.Facebook,
            //    Instagram = setting.Instagram,
            //    Pinterest = setting.Pinterest
            //};
            return StatusCode(200, getDto);
        }
    }
}
