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
    public class GenreRepository: IGenreRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public GenreRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
        }

        public async Task<List<Genre>> GetAllAsync(int page = 1, int count = 10)
        {
            return await _context.Genres.Include(x=>x.Category).Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<List<Genre>> GetAllAsync(int categoryId)
        {
            return await _context.Genres.Include(x => x.Category).Where(x=>x.CategoryId==categoryId).ToListAsync();
        }

        public async Task<Genre> GetAsync(int id)
        {
            return await _context.Genres.Include(x=>x.Books).Include(x=>x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExist(Genre genre)
        {
            return await _context.Genres.AnyAsync(x => x.Name.ToLower() == genre.Name.ToLower() && x.Id!=genre.Id);
        }

        public void Remove(Genre genre)
        {
            _context.Genres.Remove(genre);
        }
    }
}
