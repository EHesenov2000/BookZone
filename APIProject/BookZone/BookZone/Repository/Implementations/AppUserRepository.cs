using BookZone.Data;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Implementations
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _context;

        public AppUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExist(AppUser user)
        {
            return await _context.Users.AnyAsync(x => x.UserName == user.UserName);
        }
    }
}
