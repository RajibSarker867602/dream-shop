using AutoMapper;
using DreamShop.Services.ProductAPI.Models.Dtos;

namespace DreamShop.Services.ProductAPI.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
