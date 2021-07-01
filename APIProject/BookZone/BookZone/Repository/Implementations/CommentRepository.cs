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
    public class CommentRepository:ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public async Task<List<Comment>> GetAllAsync(int page = 1, int count = 10)
        {
            return await _context.Comments.Skip((page - 1) * count).Take(count).ToListAsync();
        }

        public async Task<List<Comment>> GetAllAsyncById(int id, int page = 1)
        {

            return await _context.Comments.Include(x=>x.Book).Include(x=>x.AppUser).Skip((page - 1) * 5).Take(5).Where(x=>x.BookId==id).ToListAsync();
        }
    }
}
