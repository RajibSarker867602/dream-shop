using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;

namespace DreamShop.Web.Service
{
    public class CouponService: ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> GetAllCoupons()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CouponAPIBaseUrl + "api/couponapi",
                APIType = APIType.GET
            });
        }

        public async Task<ResponseDto> GetCouponByCode(string code)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CouponAPIBaseUrl + "api/couponapi/"+code+ "/code",
                APIType = APIType.GET
            });
        }

        public async Task<ResponseDto> GetCouponById(long couponId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CouponAPIBaseUrl + "api/couponapi/" + couponId,
                APIType = APIType.GET
            });
        }

        public async Task<ResponseDto> CreateCoupon(CouponDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CouponAPIBaseUrl + "api/couponapi" ,
                APIType = APIType.POST,
                Data = requestDto
            });
        }

        public async Task<ResponseDto> UpdateCoupon(CouponDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CouponAPIBaseUrl + "api/couponapi",
                APIType = APIType.PUT,
                Data = requestDto
            });
        }

        public async Task<ResponseDto> DeleteCoupon(long couponId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CouponAPIBaseUrl + "api/couponapi/"+ couponId,
                APIType = APIType.DELETE
            });
        }
    }
}
