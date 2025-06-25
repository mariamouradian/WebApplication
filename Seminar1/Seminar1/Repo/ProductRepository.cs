using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Seminar1.Abstraction;
using Seminar1.Models;
using Seminar1.Models.DTO;

namespace Seminar1.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductRepository(IMapper mapper, IMemoryCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }
        public int AddCategory(CategoryDto category)
        {
            using (var context = new ProductContext())
            {
                var entityCategory = context.Categories.FirstOrDefault(x => x.Name.ToLower() == category.Name.ToLower());
                if (entityCategory == null)
                {
                    entityCategory = _mapper.Map<Models.Category>(category);
                    context.Categories.Add(entityCategory);
                    context.SaveChanges();
                    _cache.Remove("categories");
                }
               
                return entityCategory.Id;
            }
            
        }

        public int AddProduct(ProductDto product)
        {
            using (var context = new ProductContext())
            {
                var entityProduct = context.Products.FirstOrDefault(x => x.Name.ToLower() == product.Name.ToLower());
                if (entityProduct == null)
                {
                    entityProduct = _mapper.Map<Models.Product>(product);
                    context.Products.Add(entityProduct);
                    context.SaveChanges();
                    _cache.Remove("products");
                }
                
                return entityProduct.Id;
            }
           
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            if (_cache.TryGetValue("categories", out List<CategoryDto> categories))
            {
                return categories;
            }
            
            using (var context = new ProductContext())
            {
                var categoriesList = context.Categories.Select(x => _mapper.Map<CategoryDto>(x)).ToList();
                _cache.Set("categories", categoriesList, TimeSpan.FromMinutes(30));
                return categoriesList;
            }
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            if (_cache.TryGetValue("products", out List<ProductDto> products))
            {
                return products;
            }
            using (var context = new ProductContext())
            {
                var productsList = context.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                _cache.Set("products", productsList, TimeSpan.FromMinutes(30));
                return productsList;
            }
                
        }
    }
}
