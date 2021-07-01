using BookZone.Data.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.BookDtos
{
    public class BookCreateDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public string Desc { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ProducingPrice { get; set; }
        public bool Status { get; set; }
        public List<int> TagIds { get; set; }
    }
    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(20).NotNull().MinimumLength(1);
            RuleFor(x => x.Desc).MaximumLength(200).NotNull().MinimumLength(1);
            RuleFor(x => x.AuthorId).NotNull();
            RuleFor(x => x.GenreId).NotNull();
        }
    }
}
