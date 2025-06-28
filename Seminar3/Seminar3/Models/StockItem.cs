using Seminar3.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class StockItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int StorageId { get; set; }
    public int Quantity { get; set; }

    [ForeignKey("ProductId")]
    public virtual ProductEntity Product { get; set; } = null!;

    [ForeignKey("StorageId")]
    public virtual StorageEntity Storage { get; set; } = null!;
}