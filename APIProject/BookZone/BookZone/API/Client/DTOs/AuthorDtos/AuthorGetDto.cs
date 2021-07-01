using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.AuthorDtos
{
    public class AuthorGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int BornYear { get; set; }
    }
}
