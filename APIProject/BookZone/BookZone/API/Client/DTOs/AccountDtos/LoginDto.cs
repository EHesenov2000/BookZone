using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.AccountDtos
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginDtoValidation : AbstractValidator<LoginDto>
    {
        public LoginDtoValidation()
        {
            RuleFor(x => x.UserName).MaximumLength(20).NotNull();
            RuleFor(x => x.Password).MaximumLength(25).WithMessage("Uzunluq 25-den boyuk ola bilmez").MinimumLength(6).NotNull();
        }
    }
}
