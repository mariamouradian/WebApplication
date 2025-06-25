using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public ProductController(IProductRepository productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
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

        [HttpGet("products/export")]
        public IActionResult ExportProductsToCsv()
        {
            try
            {
                var products = _productRepository.GetProducts();

                var csv = new System.Text.StringBuilder();
                csv.AppendLine("Id,Name,Description,Cost,CategoryId");

                foreach (var product in products)
                {
                    csv.AppendLine($"{product.Id},\"{EscapeCsvField(product.Name)}\",\"{EscapeCsvField(product.Description)}\",{product.Cost},{product.CategoryId}");
                }

                return File(System.Text.Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "products.csv");
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

        private static string EscapeCsvField(string? field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            return field.Replace("\"", "\"\"");
        }

        [HttpGet("cache/stats")]
        public IActionResult GetCacheStats()
        {
            try
            {
                var stats = new
                {
                    CurrentEntryCount = _cache.GetCurrentStatistics()?.CurrentEntryCount,
                    TotalHits = _cache.GetCurrentStatistics()?.TotalHits,
                    TotalMisses = _cache.GetCurrentStatistics()?.TotalMisses
                };

                return Ok(stats);
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

        [HttpGet("cache/stats.html")]
        public IActionResult GetCacheStatsPage()
        {
            var stats = _cache.GetCurrentStatistics();
            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Cache Statistics</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        th {{ background-color: #f2f2f2; }}
    </style>
</head>
<body>
    <h1>Cache Statistics</h1>
    <table>
        <tr><th>Metric</th><th>Value</th></tr>
        <tr><td>Current Entry Count</td><td>{stats?.CurrentEntryCount ?? 0}</td></tr>
        <tr><td>Total Hits</td><td>{stats?.TotalHits ?? 0}</td></tr>
        <tr><td>Total Misses</td><td>{stats?.TotalMisses ?? 0}</td></tr>
    </table>
</body>
</html>";

            return Content(html, "text/html");
        }

        // Получение всех продуктов
        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var products = context.Products.Select(x => new ProductResponse()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Cost = x.Cost,
                        CategoryId = x.CategoryId
                    }).ToList();

                    return Ok(products);
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

        // Получение всех категорий
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var categories = context.Categories.Select(x => new CategoryResponse()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ProductCount = x.Products.Count
                    }).ToList();

                    return Ok(categories);
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

        // Добавление продукта
        [HttpPost("products")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Products.Any(x => x.Name.ToLower() == product.Name.ToLower()))
                    {
                        return Conflict(new ErrorResponse
                        {
                            Message = "Product already exists",
                            StatusCode = 409
                        });
                    }

                    context.Products.Add(product);
                    context.SaveChanges();

                    return Ok(new SuccessResponse
                    {
                        Message = "Product added successfully"
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

        // Добавление категории
        [HttpPost("categories")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Categories.Any(x => x.Name.ToLower() == category.Name.ToLower()))
                    {
                        return Conflict(new ErrorResponse
                        {
                            Message = "Category already exists",
                            StatusCode = 409
                        });
                    }

                    context.Categories.Add(category);
                    context.SaveChanges();

                    return Ok(new SuccessResponse
                    {
                        Message = "Category added successfully"
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
    
