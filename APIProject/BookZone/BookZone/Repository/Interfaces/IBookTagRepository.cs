using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface IBookTagRepository
    {
        Task CreateAsync(BookTag bookTag);
        Task<int> CommitAsync();
        Task<BookTag> GetAsync(int id);
        void Remove(BookTag bookTag);
        Task<bool> IsExist(BookTag bookTag);
    }
}
