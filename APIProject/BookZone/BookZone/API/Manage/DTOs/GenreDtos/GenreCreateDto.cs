using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.GenreDtos
{
    public class GenreCreateDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
    public class GenreCreateDtoValidator : AbstractValidator<GenreCreateDto>
    {
        public GenreCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(30).NotNull().MinimumLength(1);
        }
    }
}
