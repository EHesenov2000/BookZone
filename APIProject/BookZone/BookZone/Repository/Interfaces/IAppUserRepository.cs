using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface IAppUserRepository
    {
        Task<bool> IsExist(AppUser user);
    }
}
