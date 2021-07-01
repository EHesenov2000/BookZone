using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task CreateAsync(Category category);
        Task<int> CommitAsync();
        Task<List<Category>> GetAllAsync(int page = 1, int count = 10);
        Task<List<Category>> GetAllAsync();
        Task<Category> GetAsync(int id);
        void Remove(Category category);
        Task <bool> IsExist(Category category);
    }
}
