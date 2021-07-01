using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.AuthorDtos
{
    public class AuthorItemDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int BornYear { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
