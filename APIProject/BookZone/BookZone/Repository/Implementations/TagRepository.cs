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
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TagRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task CreateAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }
        public async Task<List<Tag>> GetAllAsync(int page = 1, int count = 10)
        {
            return await _context.Tags.Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.Where(x=>x.IsDeleted==false).ToListAsync();
        }

        public async Task<Tag> GetAsync(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExist(Tag tag)
        {
            return await _context.Tags.AnyAsync(x => x.Name.ToLower() == tag.Name.ToLower() && x.Id!=tag.Id);
        }

        //public  void Remove(Tag tag)
        //{
        //     _context.Tags.Remove(tag);
        //}
    }
}
