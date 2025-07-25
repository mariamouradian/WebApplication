﻿namespace Seminar3.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; } 
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int StorageId { get; set; }
    }
}
