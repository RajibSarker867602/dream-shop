using AutoMapper;
using DreamShop.Services.ShoppingCartAPI.Models.Dto;

namespace DreamShop.Services.ShoppingCartAPI.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
            CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
        }
    }
}
