using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Seminar3.Abstractions;
using Seminar3.Models;
using Seminar3.Models.Dto;

namespace Seminar3.Services
{
    internal class CategoryService : ICategoryService
    {

        private readonly AppDbContext? _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public int AddCategory(CategoryDto category)
        {
            using (_context)
            {
                var entity = _mapper.Map<CategoryEntity>(category);
                _context.Categories.Add(entity);
                _context.SaveChanges();
                _cache.Remove("categories");
                return entity.Id;
            }
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            using (_context)
            {
                if (_cache.TryGetValue("categories", out List<CategoryDto> categories))
                {
                    return categories;
                }
                categories = _context.Categories.Select(x => _mapper.Map<CategoryDto>(x)).ToList();
                _cache.Set("categories", categories, TimeSpan.FromMinutes(30));
                return categories;
            }
        }
    }
}