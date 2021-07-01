using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.DTOs.AccountDtos
{
    public class ClientAccountGetDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
