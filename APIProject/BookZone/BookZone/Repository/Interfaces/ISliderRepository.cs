using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Interfaces
{
    public interface ISliderRepository
    {
        Task CreateAsync(Slider slider);
        Task<int> CommitAsync();
        Task<List<Slider>> GetAllAsync(int page=1,int count=10);
        Task<List<Slider>> GetAllAsync();
        Task<Slider> GetAsync(int id);
        void Remove(Slider slider);

    }
}
