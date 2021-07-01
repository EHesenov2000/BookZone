using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.CommentDtos
{
    public class CommentCreateDto
    {
        public string Text { get; set; }
        public int BookId { get; set; }
    }
    public class CommentCreateDtoValidation:AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidation()
        {
            RuleFor(x => x.Text).MaximumLength(500).NotNull().MinimumLength(1);
            RuleFor(x => x.BookId).NotNull();
        }
    }
}
