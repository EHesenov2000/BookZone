using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Data.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int BornYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<Book> Books { get; set; }

    }
}
