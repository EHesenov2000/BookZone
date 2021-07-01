using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.AccountDtos
{
    public class AccountUpdateDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
    public class AccountUpdateDtoValidation : AbstractValidator<AccountUpdateDto>
    {
        public AccountUpdateDtoValidation()
        {
            RuleFor(x => x.FullName).MaximumLength(20).WithMessage("Uzunluq max 20 ola biler!");
            RuleFor(x => x.UserName).MaximumLength(20).WithMessage("Uzunluq max 20 ola biler!");
            RuleFor(x => x.UserName).MinimumLength(6).WithMessage("Uzunluq min 6 ola biler!");

        }
    }
}
