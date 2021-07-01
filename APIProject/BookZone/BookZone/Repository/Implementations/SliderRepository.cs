using BookZone.Data;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.Repository.Implementations
{
    public class SliderRepository : ISliderRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
        }

        public async Task<List<Slider>> GetAllAsync(int page = 1, int count = 10)
        {
            return await _context.Sliders.Skip((page-1)*count).Take(count).ToListAsync();
        }

        public async Task<List<Slider>> GetAllAsync()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<Slider> GetAsync(int id)
        {
            return await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public  void Remove(Slider slider)
        {

             _context.Sliders.Remove(slider);
            string rootPath = _env.WebRootPath;
            var path = Path.Combine(rootPath, "uploads/slider", slider.Image);
            System.IO.File.Delete(path);

        }

    }
}
