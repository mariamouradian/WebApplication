using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seminar1.Abstraction;
using Seminar1.Models;
using Seminar1.Models.DTO;

namespace Seminar1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        // Модели ответов
        public class ProductResponse : BaseModel
        {
            public int Cost { get; set; }
            public int CategoryId { get; set; }
        }

        public class CategoryResponse : BaseModel
        {
            public int ProductCount { get; set; }
        }

        public class ErrorResponse
        {
            public required string Message { get; set; }
            public int StatusCode { get; set; }
        }

        public class SuccessResponse
        {
            public required string Message { get; set; }
        }

        // Получение всех продуктов
        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            return Ok(products);
        }

        // Получение всех категорий
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _productRepository.GetCategories();
            return Ok(categories);
        }

        // Добавление продукта
        [HttpPost("products")]
        public IActionResult AddProduct([FromBody] ProductDto productDto)
        {
            var result = _productRepository.AddProduct(productDto);
            return Ok(result);
        }

        // Добавление категории
        [HttpPost("categories")]
        public IActionResult AddCategory([FromBody] CategoryDto categoryDto)
        {
            var result = _productRepository.AddCategory(categoryDto);
            return Ok(result);
        }

        // Удаление продукта
        [HttpDelete("products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var product = context.Products.Find(id);
                    if (product == null)
                    {
                        return NotFound(new ErrorResponse
                        {
                            Message = "Product not found",
                            StatusCode = 404
                        });
                    }

                    context.Products.Remove(product);
                    context.SaveChanges();

                    return Ok(new SuccessResponse
                    {
                        Message = "Product deleted successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 500
                });
            }
        }

        // Удаление категории
        [HttpDelete("categories/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var category = context.Categories
                        .Include(c => c.Products)
                        .FirstOrDefault(c => c.Id == id);

                    if (category == null)
                    {
                        return NotFound(new ErrorResponse
                        {
                            Message = "Category not found",
                            StatusCode = 404
                        });
                    }

                    if (category.Products.Any())
                    {
                        return BadRequest(new ErrorResponse
                        {
                            Message = "Cannot delete category with products",
                            StatusCode = 400
                        });
                    }

                    context.Categories.Remove(category);
                    context.SaveChanges();

                    return Ok(new SuccessResponse
                    {
                        Message = "Category deleted successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 500
                });
            }
        }

        // Обновление цены продукта
        [HttpPatch("products/{id}/price")]
        public IActionResult UpdateProductPrice(int id, [FromBody] int newPrice)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var product = context.Products.Find(id);
                    if (product == null)
                    {
                        return NotFound(new ErrorResponse
                        {
                            Message = "Product not found",
                            StatusCode = 404
                        });
                    }

                    product.Cost = newPrice;
                    context.SaveChanges();

                    return Ok(new SuccessResponse
                    {
                        Message = "Product price updated successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = ex.Message,
                    StatusCode = 500
                });
            }
        }
    }
}