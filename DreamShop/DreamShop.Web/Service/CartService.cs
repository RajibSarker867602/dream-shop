using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;

namespace DreamShop.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetCartsByUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CartAPIBaseUrl + "api/CartAPI/" + userId + "/carts",
                APIType = APIType.GET
            });
        }

        public async Task<ResponseDto> AddCoupon(CartHeaderDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CartAPIBaseUrl + "api/cartapi/addCoupon",
                APIType = APIType.POST,
                Data = cart
            });
        }

        public async Task<ResponseDto> UpsertCart(CartHeaderDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CartAPIBaseUrl + "api/cartapi/upsert",
                APIType = APIType.POST,
                Data = cart
            });
        }

        public async Task<ResponseDto> RemoveCart(long cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.CartAPIBaseUrl + "api/cartapi/removeCart",
                APIType = APIType.POST,
                Data = cartDetailsId
            });
        }
    }
}