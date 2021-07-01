using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.DTOs.TagDtos
{
    public class TagItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAT { get; set; }
        public bool IsDeleted { get; set; }
    }
}
