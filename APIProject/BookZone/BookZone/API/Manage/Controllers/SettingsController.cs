using BookZone.API.Manage.DTOs.SettingDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class SettingsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ISettingRepository _settingRepository;

        public SettingsController(IWebHostEnvironment env, ISettingRepository settingRepository)
        {
            _env = env;
            _settingRepository = settingRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SettingCreateDto setting)
        {
            if (await _settingRepository.GetSettingCount() == 1) return BadRequest();
            if (setting == null) return BadRequest();

            Setting newSetting = new Setting { };
            if (setting.File != null)
            {
                if (setting.File.ContentType != "image/png" && setting.File.ContentType != "image/jpeg")
                {
                    return Conflict();
                }
                if (setting.File.Length > (1024 * 1024) * 5)
                {
                    return Conflict();
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + setting.File.FileName;
                var path = Path.Combine(rootPath, "uploads/setting", fileName);
                using (System.IO.FileStream stream = new FileStream(path, System.IO.FileMode.Create))
                {
                    setting.File.CopyTo(stream);
                }
                newSetting.Image = fileName;
            }

            else
            {
                return BadRequest();
            }
            newSetting.Location = setting.Location;
            newSetting.Contact = setting.Contact;
            newSetting.Email = setting.Contact;
            newSetting.Facebook = setting.Facebook;
            newSetting.Instagram = setting.Instagram;
            newSetting.Pinterest = setting.Pinterest;

            await _settingRepository.CreateAsync(newSetting);
            await _settingRepository.CommitAsync();
            return StatusCode(201);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Setting settingDatabase = await _settingRepository.GetAsync(id);
            if (settingDatabase == null) return NotFound();
            SettingGetDto setting = new SettingGetDto
            {
                Id = settingDatabase.Id,
                Image = settingDatabase.Image,
                Location=settingDatabase.Location,
                Contact=settingDatabase.Contact,
                Email=settingDatabase.Email,
                Facebook=settingDatabase.Facebook,
                Instagram=settingDatabase.Instagram,
                Pinterest=settingDatabase.Pinterest
            };
            return Ok(setting);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _settingRepository.GetAsync(id) == null) return NotFound();
            _settingRepository.Remove(await _settingRepository.GetAsync(id));
            return Ok(await _settingRepository.CommitAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] SettingCreateDto createDto,  int id)
        {
            if (createDto == null || createDto.File == null) return Conflict();
            Setting existSetting= await _settingRepository.GetAsync(id);
            if (existSetting == null) return NotFound();

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
            var path = Path.Combine(rootPath, "uploads/setting", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createDto.File.CopyTo(stream);
            }
            if (existSetting.Image != null)
            {
                string existPath = Path.Combine(_env.WebRootPath, "uploads/setting", existSetting.Image);
                if (System.IO.File.Exists(existPath))
                {
                    System.IO.File.Delete(existPath);
                }
            }
            existSetting.Image = fileName;
            existSetting.Location = createDto.Location;
            existSetting.Contact = createDto.Contact;
            existSetting.Email = createDto.Email;
            existSetting.Facebook = createDto.Facebook;
            existSetting.Instagram = createDto.Instagram;
            existSetting.Pinterest = createDto.Pinterest;

            await _settingRepository.CommitAsync();
            return StatusCode(200);

        }
    }
}
