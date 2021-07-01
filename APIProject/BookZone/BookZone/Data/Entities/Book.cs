using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string Image { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ProducingPrice { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Author Author { get; set; }
        public Genre Genre { get; set; }
        public List<BookTag> BookTags { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
