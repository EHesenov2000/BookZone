using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface ISettingRepository
    {
        Task CreateAsync(Setting setting);
        Task<int> CommitAsync();
        Task<int> GetSettingCount();
        Task<Setting> GetAsync(int id);
        Task<Setting> GetAsync();
        void Remove(Setting setting);
    }
}
