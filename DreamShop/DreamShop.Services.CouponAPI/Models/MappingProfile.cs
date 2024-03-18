using AutoMapper;
using DreamShop.Services.CouponAPI.Models.DTOs;
using DreamShop.Services.CouponAPI.Models.Entities;

namespace DreamShop.Services.CouponAPI.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Coupon, CouponDto>();
            CreateMap<CouponDto, Coupon>();
        }
    }
}
