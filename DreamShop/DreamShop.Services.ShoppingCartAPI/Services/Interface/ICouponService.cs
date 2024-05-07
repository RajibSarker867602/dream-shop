using DreamShop.Services.ShoppingCartAPI.Models.DTOs;

namespace DreamShop.Services.ShoppingCartAPI.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponByCode(string code);
        //Task<ResponseDto> GetCouponById(long couponId);
        //Task<ResponseDto> CreateCoupon(CouponDto requestDto);
        //Task<ResponseDto> UpdateCoupon(CouponDto requestDto);
        //Task<ResponseDto> DeleteCoupon(long couponId);
    }
}
