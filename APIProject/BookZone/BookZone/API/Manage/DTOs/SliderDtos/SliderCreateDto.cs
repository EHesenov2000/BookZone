using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.SliderDtos
{
    public class SliderCreateDto
    {
        public IFormFile File { get; set; }
    }
    public class SliderCreateDtoValidation:AbstractValidator<SliderCreateDto>
    {
        public SliderCreateDtoValidation()
        {
        }
    }
}
