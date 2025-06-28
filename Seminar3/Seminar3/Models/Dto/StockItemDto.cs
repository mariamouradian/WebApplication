public class StockItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int StorageId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; } = null!;
    public string StorageName { get; set; } = null!;
}