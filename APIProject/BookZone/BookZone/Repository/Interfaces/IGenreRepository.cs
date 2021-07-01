using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface IGenreRepository
    {
        Task CreateAsync(Genre genre);
        Task<int> CommitAsync();
        Task<List<Genre>> GetAllAsync(int page = 1, int count = 10);
        Task<List<Genre>> GetAllAsync(int categoryId);
        Task<Genre> GetAsync(int id);
        void Remove(Genre genre);
        Task<bool> IsExist(Genre genre);

    }
}
