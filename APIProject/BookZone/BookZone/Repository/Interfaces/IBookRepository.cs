using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface IBookRepository
    {
        Task CreateAsync(Book book);
        Task<int> CommitAsync();
        Task<List<Book>> GetAllAsync(int page = 1, int count = 10);
        Task<List<Book>> GetAllAsync(int? categoryId, int? genreId);

        Task<Book> GetAsync(int id);
        //void Remove(Book book);
    }
}
