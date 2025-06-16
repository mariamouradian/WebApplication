namespace Seminar1.Models
{
    public class ProductStorage
    {
        public int? ProductID { get; set; }
        public int? StorageID { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Storage? Storage { get; set; }
    }
}
