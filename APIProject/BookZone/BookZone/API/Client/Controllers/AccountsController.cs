using AutoMapper;
using BookZone.API.Client.DTOs.AccountDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookZone.API.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAppUserRepository _appUserRepository;

        public AccountsController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,IAppUserRepository appUserRepository,IConfiguration configuration, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateDto accountCreateDto)
        {
            if (accountCreateDto == null || accountCreateDto.UserName == null || accountCreateDto.FullName == null) return BadRequest();
            AppUser user = new AppUser
            {
                IsAdmin = false,
                UserName = accountCreateDto.UserName,
                FullName = accountCreateDto.FullName
            };
            if (await _appUserRepository.IsExist(user)) return Conflict();


            user.IsAdmin = false;
            await _userManager.CreateAsync(user, accountCreateDto.Password);
            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            AppUser admin = await _userManager.FindByNameAsync(loginDto.UserName);

            if (admin == null) return NotFound();

            if (!(await _userManager.CheckPasswordAsync(admin, loginDto.Password))) return Unauthorized();


            #region Jwt generate

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id),
                new Claim(ClaimTypes.Name, admin.UserName),
                new Claim(JwtRegisteredClaimNames.Sub,admin.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(admin);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            claims.AddRange(roleClaims);

            //foreach (var roleName in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, roleName));
            //}

            string key = _configuration.GetSection("JWT:SecurityKey").Value;

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken
                (
                    issuer: _configuration.GetSection("JWT:Issuer").Value,
                    audience: _configuration.GetSection("JWT:Issuer").Value,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(3),
                    signingCredentials: creds
                );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            #endregion

            return Ok(new { token = tokenStr });
        }


        [Authorize(Roles = "Member,SuperAdmin")]
        public async Task<IActionResult> Get()
        {
            AppUser admin = await _userManager.FindByNameAsync(User.Identity.Name);

            if (admin == null)
            {
                return Unauthorized();
            }

            //AccountGetDto accountDto = new AccountGetDto
            //{
            //    FullName = admin.FullName,
            //    UserName = admin.UserName
            //};
            ClientAccountGetDto accountDto = _mapper.Map<ClientAccountGetDto>(admin);

            return Ok(accountDto);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(AccountUpdateDto accountDto)
        {
            AppUser account = await _userManager.FindByNameAsync(User.Identity.Name);
            account.UserName = accountDto.UserName;
            account.FullName = accountDto.FullName;

            if (account == null) return Unauthorized();

            if (_userManager.Users.Any(x => x.Id != account.Id && x.NormalizedUserName == accountDto.UserName.ToUpper())) return Conflict();

            var result1=await _userManager.ChangePasswordAsync(account, accountDto.CurrentPassword, accountDto.NewPassword);
            var result = await _userManager.UpdateAsync(account);
            if (result1.Succeeded == false) return Conflict(new { message = result.Errors.First().Description });
            if (result.Succeeded == false) return BadRequest(new { message = result.Errors.First().Description });
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return Unauthorized();
            var result=await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return Conflict();
            return Ok();
        }
    }
}
