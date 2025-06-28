using Seminar3.Models.Dto;

namespace Seminar3.Abstractions
{
    public interface IProductRepository
    {
        public int AddCategory(CategoryDto category);

        public IEnumerable<CategoryDto> GetCategories();

        public int AddProduct(ProductDto product);

        public IEnumerable<ProductDto> GetProducts();
    }
}
