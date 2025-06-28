using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Seminar3.Abstractions;
using Seminar3.Models;
using Seminar3.Models.Dto;

namespace Seminar3.Mapper
{
    public class MyRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;

        public MyRepository(IMapper mapper, IMemoryCache cache, AppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int AddCategory(CategoryDto category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            if (string.IsNullOrEmpty(category.Name)) throw new ArgumentException("Category name is required");

            var entityCategory = _context.Categories
                .FirstOrDefault(x => x.Name != null &&
                                   x.Name.ToLower() == category.Name.ToLower());

            if (entityCategory == null)
            {
                entityCategory = _mapper.Map<CategoryEntity>(category);
                _context.Categories.Add(entityCategory);
                _context.SaveChanges();
                _cache.Remove("categories");
            }

            return entityCategory.Id;
        }

        public int AddProduct(ProductDto product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (string.IsNullOrEmpty(product.Name)) throw new ArgumentException("Product name is required");

            var entityProduct = _context.Products
                .FirstOrDefault(x => x.Name != null &&
                                   x.Name.ToLower() == product.Name.ToLower());

            if (entityProduct == null)
            {
                entityProduct = _mapper.Map<ProductEntity>(product);
                _context.Products.Add(entityProduct);
                _context.SaveChanges();
                _cache.Remove("products");
            }

            return entityProduct.Id;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            if (_cache.TryGetValue("categories", out List<CategoryDto>? categories) && categories != null)
            {
                return categories;
            }

            var categoriesList = _context.Categories
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name ?? string.Empty,
                    Description = x.Description ?? string.Empty,
                })
                .ToList();

            _cache.Set("categories", categoriesList, TimeSpan.FromMinutes(30));
            return categoriesList;
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            if (_cache.TryGetValue("products", out List<ProductDto>? products) && products != null)
            {
                return products;
            }

            var productsList = _context.Products
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name ?? string.Empty,
                    Description = x.Description ?? string.Empty,
                    Price = x.Price,
                    CategoryId = x.CategoryId
                })
                .ToList();

            _cache.Set("products", productsList, TimeSpan.FromMinutes(30));
            return productsList;
        }
    }
}