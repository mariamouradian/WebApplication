using Seminar3.Models.Dto;

namespace Seminar3.Abstractions
{
    public interface IStockService
    {
        IEnumerable<StockItemDto> GetStockItems();
        StockItemDto GetStockItem(int productId, int storageId);
        int AddStockItem(StockItemDto stockItem);
        void UpdateStockItem(StockItemDto stockItem);
    }
}