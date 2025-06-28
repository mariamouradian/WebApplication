using HotChocolate;
using Seminar3.Abstractions;
using Seminar3.Models.Dto;

namespace Seminar3.Query
{
    public class StockQuery
    {
        public IEnumerable<StockItemDto> GetStockItems([Service] IStockService service) =>
            service.GetStockItems();

        public StockItemDto GetStockItem(
            int productId,
            int storageId,
            [Service] IStockService service) =>
            service.GetStockItem(productId, storageId);
    }
}