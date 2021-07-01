using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.SettingDtos
{
    public class SettingCreateDto
    {
        public IFormFile File { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Pinterest { get; set; }
    }
    public class SettingCreateDtoValidation:AbstractValidator<SettingCreateDto>
    {
        public SettingCreateDtoValidation()
        {
            RuleFor(x => x.Location).MaximumLength(100).NotNull().MinimumLength(1);
            RuleFor(x => x.Contact).MaximumLength(100).NotNull().MinimumLength(1);
            RuleFor(x => x.Email).MaximumLength(100).NotNull();
            RuleFor(x => x.Facebook).MaximumLength(100).NotNull();
            RuleFor(x => x.Instagram).MaximumLength(100).NotNull();
            RuleFor(x => x.Pinterest).MaximumLength(100).NotNull();
        }
    }
}
