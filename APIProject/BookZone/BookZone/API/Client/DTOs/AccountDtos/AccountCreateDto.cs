using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.AccountDtos
{
    public class AccountCreateDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
    public class AccountCreateDtoValidation : AbstractValidator<AccountCreateDto>
    {
        public AccountCreateDtoValidation()
        {
            RuleFor(x => x.FullName).MaximumLength(20).WithMessage("Uzunluq max 20 ola biler!");
            RuleFor(x => x.UserName).MaximumLength(20).WithMessage("Uzunluq max 20 ola biler!");

        }
    }
}
