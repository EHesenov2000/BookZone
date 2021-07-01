using AutoMapper;
using BookZone.API.Manage.DTOs.AccountDtos;
using BookZone.Data.Entities;
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

namespace BookZone.API.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountsController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration,IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }
        //public async Task<IActionResult> Roles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    return Ok();
        //}


        //public async Task<IActionResult> Create()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        UserName = "SuperAdmin",
        //        FullName = "Super Admin",
        //        IsAdmin = true
        //    };

        //    await _userManager.CreateAsync(appUser, "admin123");
        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Ok();
        //}

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


        [Authorize(Roles = "SuperAdmin")]
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
            AccountGetDto accountDto = _mapper.Map<AccountGetDto>(admin);

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

            var result = await _userManager.UpdateAsync(account);

            if (result.Succeeded == false) return BadRequest(new { message = result.Errors.First().Description });

            return NoContent();
        }
    }
}
