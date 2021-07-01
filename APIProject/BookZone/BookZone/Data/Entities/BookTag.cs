using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Data.Entities
{
    public class BookTag
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int TagId { get; set; }

        public Book Book { get; set; }
        public Tag Tag { get; set; }
    }
}
