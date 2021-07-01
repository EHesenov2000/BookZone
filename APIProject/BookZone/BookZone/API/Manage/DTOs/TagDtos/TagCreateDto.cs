using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.TagDtos
{
    public class TagCreateDto
    {
        public string Name { get; set; }
    }
    public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
    {
        public TagCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(20).NotNull().MinimumLength(1);
        }
    }
}
