using Seminar3.Models;
using Seminar3.Models.Dto;

namespace Seminar3.Abstractions
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetProducts();
        int AddProduct(ProductDto product);

    }
}
