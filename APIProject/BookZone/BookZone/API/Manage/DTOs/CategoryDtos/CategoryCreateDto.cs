using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.CategoryDtos
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
    }
    public class CategoryCreateDtoValidation: AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidation()
        {
            RuleFor(x => x.Name).MaximumLength(30).NotNull().MinimumLength(1);
        }
    }
}
