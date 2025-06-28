using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Seminar3.Abstractions;
using Seminar3.Models;
using Seminar3.Models.Dto;

namespace Seminar3.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductService(AppDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public int AddProduct(ProductDto product)
        {
            try
            {
                // Проверяем существование категории и склада
                if (!_context.Categories.Any(c => c.Id == product.CategoryId))
                    throw new ArgumentException($"Category with id {product.CategoryId} not found");

                if (!_context.Storages.Any(s => s.Id == product.StorageId))
                    throw new ArgumentException($"Storage with id {product.StorageId} not found");

                var entity = new ProductEntity
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    StorageId = product.StorageId
                };

                _context.Products.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
            catch (DbUpdateException ex)
            {
                // Логируем внутреннюю ошибку
                Console.WriteLine($"DB Error: {ex.InnerException?.Message}");
                throw new Exception("Database save error", ex);
            }
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            if (_cache.TryGetValue("products", out List<ProductDto>? products) && products != null)
            {
                return products;
            }

            products = _context.Products
                .Select(x => _mapper.Map<ProductDto>(x))
                .ToList();

            _cache.Set("products", products, TimeSpan.FromMinutes(30));
            return products;
        }
    }
}