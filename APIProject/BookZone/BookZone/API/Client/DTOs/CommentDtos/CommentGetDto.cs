using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.CommentDtos
{
    public class CommentGetDto
    {
        public string Text { get; set; }
        public string AppUserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
