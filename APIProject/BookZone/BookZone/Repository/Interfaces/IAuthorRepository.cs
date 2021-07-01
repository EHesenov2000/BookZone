using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface IAuthorRepository
    {
        Task CreateAsync(Author author);
        Task<int> CommitAsync();
        Task<List<Author>> GetAllAsync(int page = 1, int count = 10);
        Task<List<Author>> GetAllAsync();

        Task<bool> IsExist(Author author);
        Task<Author> GetAsync(int id);
        //void Remove(Author author);
    }
}
