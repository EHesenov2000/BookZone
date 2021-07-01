using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface ICommentRepository
    {
        Task CreateAsync(Comment comment);
        Task<int> CommitAsync();
        Task<List<Comment>> GetAllAsync(int page = 1, int count = 10);
        Task<List<Comment>> GetAllAsyncById(int id,int page = 1);
    }
}
