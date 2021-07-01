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
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task<List<Book>> GetAllAsync(int page = 1, int count = 10)
        {
            return await _context.Books.Include(x=>x.Author).Include(x=>x.Genre).ThenInclude(x=>x.Category).Include(x=>x.BookTags).ThenInclude(x=>x.Tag).Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<List<Book>> GetAllAsync(int? categoryId, int? genreId)
        {
            List<Book> books = await _context.Books.Include(x => x.Author).Include(x => x.BookTags).ThenInclude(x => x.Tag).Include(x => x.Genre).ThenInclude(x => x.Category).Where(x=>x.IsDeleted==false).ToListAsync();
            if (categoryId != null)
            {
                books=books.Where(x => x.Genre.CategoryId == categoryId ).ToList();
            }
            if (genreId != null)
            {
                books=books.Where(x => x.GenreId == genreId).ToList();
            }
            return books;
        }

        public async Task<Book> GetAsync(int id)
        {
            return await _context.Books.Include(x => x.Author).Include(x => x.Genre).ThenInclude(x=>x.Category).Include(x=>x.BookTags).ThenInclude(x=>x.Tag).FirstOrDefaultAsync(x => x.Id == id);
        }

        //public void Remove(Book book)
        //{
        //    _context.Books.Remove(book);
        //    string existPath = Path.Combine(_env.WebRootPath, "uploads/book", book.Image);
        //    if (System.IO.File.Exists(existPath))
        //    {
        //        System.IO.File.Delete(existPath);
        //    }
        //}
    }
}
