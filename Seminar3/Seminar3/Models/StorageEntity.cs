namespace Seminar3.Models
{
    public class StorageEntity
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Description { get; set; }
        public virtual ICollection<ProductEntity> ?Products { get; set; }
    }
}