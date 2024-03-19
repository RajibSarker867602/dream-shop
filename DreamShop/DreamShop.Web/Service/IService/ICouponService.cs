using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;

namespace DreamShop.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetAllCoupons();
        Task<ResponseDto> GetCouponByCode(string code);
        Task<ResponseDto> GetCouponById(long couponId);
        Task<ResponseDto> CreateCoupon(CouponDto requestDto);
        Task<ResponseDto> UpdateCoupon(CouponDto requestDto);
        Task<ResponseDto> DeleteCoupon(long couponId);
    }
}
