using HotChocolate;
using Seminar3.Abstractions;
using Seminar3.Models;
using Seminar3.Models.Dto;

namespace Seminar3.Mutation
{
    public class MySimpleMutation
    {
        public record AddProductPayload(int Id);

        public AddProductPayload AddProduct(
            ProductEntity product,
            [Service] IProductService service)
        {
            try
            {
                var id = service.AddProduct(new ProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    StorageId = product.StorageId
                });
                return new AddProductPayload(id);
            }
            catch (Exception ex)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("Failed to add product")
                        .SetCode("PRODUCT_ADD_FAILED")
                        .SetExtension("details", ex.Message)
                        .Build());
            }
        }
    }
}