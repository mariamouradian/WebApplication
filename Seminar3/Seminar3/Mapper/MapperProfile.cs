using AutoMapper;
using Seminar3.Models;
using Seminar3.Models.Dto;
namespace Seminar3.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProductEntity, ProductDto>().ReverseMap();
            CreateMap<StorageEntity, StorageDto>().ReverseMap();
            CreateMap<CategoryEntity, CategoryDto>().ReverseMap();
        }
    }
}
