namespace Seminar3.Models.Dto
{
    public class ProductInput
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int StorageId { get; set; }
    }
}