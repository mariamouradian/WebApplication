using AutoMapper;
using Seminar1.Models;
using Seminar1.Models.DTO;

namespace Seminar1.Repo
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination).ReverseMap();

            CreateMap<Category, CategoryDto>(MemberList.Destination).ReverseMap();

            CreateMap<Storage, StorageDto>(MemberList.Destination).ReverseMap();
        }

    }
}
