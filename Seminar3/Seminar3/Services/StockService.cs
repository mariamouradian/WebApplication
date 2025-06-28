using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Seminar3.Abstractions;
using Seminar3.Models;
using Seminar3.Models.Dto;

namespace Seminar3.Services
{
    public class StockService : IStockService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StockService(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<StockItemDto> GetStockItems()
        {
            return _context.StockItems
                .Include(s => s.Product)
                .Include(s => s.Storage)
                .Where(s => s.Product != null && s.Storage != null)
                .Select(s => new StockItemDto
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    StorageId = s.StorageId,
                    Quantity = s.Quantity,
                    ProductName = s.Product!.Name,
                    StorageName = s.Storage!.Name
                }).ToList();
        }

        public StockItemDto GetStockItem(int productId, int storageId)
        {
            var item = _context.StockItems
                .Include(s => s.Product)
                .Include(s => s.Storage)
                .FirstOrDefault(s => s.ProductId == productId && s.StorageId == storageId);

            return _mapper.Map<StockItemDto>(item);
        }

        public int AddStockItem(StockItemDto stockItem)
        {
            var entity = _mapper.Map<StockItem>(stockItem);
            _context.StockItems.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public void UpdateStockItem(StockItemDto stockItem)
        {
            var existing = _context.StockItems.Find(stockItem.Id);
            if (existing != null)
            {
                _mapper.Map(stockItem, existing);
                _context.SaveChanges();
            }
        }
    }
}