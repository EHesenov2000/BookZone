using BookZone.API.Client.DTOs.TagDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.BookDtos
{
    public class BookGetDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public decimal SalePrice { get; set; }
        public bool Status { get; set; }
        public  List<TagGetDto> Tags { get; set; }
    }
}
