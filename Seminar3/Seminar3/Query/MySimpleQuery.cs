using HotChocolate;
using Seminar3.Abstractions;
using Seminar3.Models.Dto;

namespace Seminar3.Query
{
    public class MySimpleQuery
    {
        public IEnumerable<ProductDto> GetProducts([Service] IProductService service) =>
            service.GetProducts();

        public IEnumerable<StorageDto> GetStorages([Service] IStorageService service) =>
            service.GetStorages();

        public IEnumerable<CategoryDto> GetCategories([Service] ICategoryService service) =>
            service.GetCategories();

        public IEnumerable<StockItemDto> GetStockItems([Service] IStockService service) =>
            service.GetStockItems();

    }
}