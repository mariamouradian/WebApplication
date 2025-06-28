using Seminar3.Models.Dto;

namespace Seminar3.Abstractions
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetCategories();
        int AddCategory(CategoryDto category);
    }
}