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
    public class BookTagRepository : IBookTagRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookTagRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(BookTag bookTag)
        {
            await _context.BookTags.AddAsync(bookTag);
        }

        public async Task<BookTag> GetAsync(int id)
        {
            return await _context.BookTags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExist(BookTag bookTag)
        {
            return await _context.BookTags.AnyAsync(x => x.BookId == bookTag.BookId && x.TagId==bookTag.TagId);
        }

        public void Remove(BookTag bookTag)
        {
            _context.BookTags.Remove(bookTag);
        }
        public void RemoveForBook(Book book)
        {
            _context.BookTags.Remove(_context.BookTags.FirstOrDefault(x=>x.Id==book.Id));
        }
    }
}
