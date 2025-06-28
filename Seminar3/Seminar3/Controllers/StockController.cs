using Microsoft.AspNetCore.Mvc;
using Seminar3.Abstractions;
using Seminar3.Models.Dto;

namespace Seminar3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;

        public StockController(IStockService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetStockItems());
        }

        [HttpGet("{productId}/{storageId}")]
        public IActionResult Get(int productId, int storageId)
        {
            var item = _service.GetStockItem(productId, storageId);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public IActionResult Add([FromBody] StockItemDto stockItem)
        {
            var id = _service.AddStockItem(stockItem);
            return CreatedAtAction(nameof(Get), new { productId = stockItem.ProductId, storageId = stockItem.StorageId }, id);
        }

        [HttpPut]
        public IActionResult Update([FromBody] StockItemDto stockItem)
        {
            _service.UpdateStockItem(stockItem);
            return NoContent();
        }
    }
}