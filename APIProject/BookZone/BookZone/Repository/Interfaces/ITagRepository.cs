using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface ITagRepository
    {
        Task CreateAsync(Tag tag);
        Task<int> CommitAsync();
        Task<List<Tag>> GetAllAsync(int page = 1, int count = 10);
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetAsync(int id);
        Task<bool> IsExist(Tag tag);
        //void Remove(Tag tag);
    }
}
