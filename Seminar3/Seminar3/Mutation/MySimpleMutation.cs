using HotChocolate;
using Seminar3.Abstractions;
using Seminar3.Models.Dto;

namespace Seminar3.Mutation
{
    public class MySimpleMutation
    {
        public record AddProductPayload(int Id);

        public AddProductPayload AddProduct(
            ProductInput productInput,
            [Service] IProductService service)
        {
            try
            {
                var id = service.AddProduct(new ProductDto
                {
                    Name = productInput.Name,
                    Description = productInput.Description,
                    Price = productInput.Price,
                    CategoryId = productInput.CategoryId,
                    StorageId = productInput.StorageId
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