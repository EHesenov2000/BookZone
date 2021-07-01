using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Data.Entities
{
    public class Setting
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile File { get; set; } 
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Pinterest { get; set; }
    }
}
