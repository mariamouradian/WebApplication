using Seminar1.Models.DTO;

namespace Seminar1.Abstraction
{
    public interface IProductRepository
    {
        public int AddCategory(CategoryDto category);

        public IEnumerable<CategoryDto> GetCategories();

        public int AddProduct(ProductDto product);

        public IEnumerable<ProductDto> GetProducts();
    }
}
