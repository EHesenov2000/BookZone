using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.TagDtos
{

    public class TagUpdateDto
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
    {
        public TagUpdateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(20).NotNull().MinimumLength(1);
        }
    }
}
