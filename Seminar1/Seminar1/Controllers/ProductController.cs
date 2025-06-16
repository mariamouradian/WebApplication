using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seminar1.Models;

namespace Seminar1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
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
            public string Message { get; set; }
            public int StatusCode { get; set; }
        }

        public class SuccessResponse
        {
            public string Message { get; set; }
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