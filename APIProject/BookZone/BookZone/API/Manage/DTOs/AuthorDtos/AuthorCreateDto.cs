using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.AuthorDtos
{
    public class AuthorCreateDto
    {
        public string FullName { get; set; }
        public int BornYear { get; set; }
    }

    public class AuthorCreateDtoValidator : AbstractValidator<AuthorCreateDto>
    {
        public AuthorCreateDtoValidator()
        {
            RuleFor(x => x.FullName).MaximumLength(30).MinimumLength(2);
            RuleFor(x => x.BornYear).LessThanOrEqualTo(2010);
        }
    }
}
