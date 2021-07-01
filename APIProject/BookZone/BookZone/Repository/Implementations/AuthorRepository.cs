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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AuthorRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public async Task<List<Author>> GetAllAsync(int page = 1, int count = 10)
        {
            return await _context.Authors.Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.Where(x=>x.IsDeleted==false).ToListAsync();
        }

        public async Task<Author> GetAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExist(Author author)
        {
            return await _context.Authors.AnyAsync(x => x.FullName.ToLower() == author.FullName.ToLower() && x.Id != author.Id);
        }

        //public void Remove(Author author)
        //{
        //    _context.Authors.Remove(author);
        //}
    }
}
