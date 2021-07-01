using BookZone.Data;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Implementations
{
    public class CategoryRepository:ICategoryRepository

    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task<List<Category>> GetAllAsync(int page = 1, int count = 10)
        {
            return await _context.Categories.Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetAsync(int id)
        {
            return await _context.Categories.Include(x=>x.Genres).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExist(Category category)
        {
            return await _context.Categories.AnyAsync(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim() && x.Id!=category.Id);
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }
    }
}
