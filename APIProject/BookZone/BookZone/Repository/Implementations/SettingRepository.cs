using BookZone.Data;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Implementations
{
    public class SettingRepository : ISettingRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Setting setting)
        {
            await _context.Settings.AddAsync(setting);
        }

        public async Task<Setting> GetAsync(int id)
        {
            return await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Setting> GetAsync()
        {
            return await _context.Settings.FirstAsync();
        }

        public async Task<int> GetSettingCount()
        {
            return await _context.Settings.CountAsync();
        }

        public void Remove(Setting setting)
        {
            _context.Settings.Remove(setting);
            string existPath = Path.Combine(_env.WebRootPath, "uploads/setting", setting.Image);
            if (System.IO.File.Exists(existPath))
            {
                System.IO.File.Delete(existPath);
            }
        }
    }
}
