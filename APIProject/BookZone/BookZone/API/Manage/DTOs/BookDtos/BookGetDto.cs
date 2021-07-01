using BookZone.API.Manage.DTOs.TagDtos;
using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.BookDtos
{
    public class BookGetDto
    {
        public int Id { get; set; }
        public string AuthorName{ get; set; }
        public string GenreName { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Desc { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ProducingPrice { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<TagItemDto> Tags { get; set; }
    }
}
