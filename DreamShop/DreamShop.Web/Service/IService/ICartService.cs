using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;

namespace DreamShop.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartsByUserIdAsync(string userId);
        Task<ResponseDto> AddCoupon(CartHeaderDto cart);
        Task<ResponseDto> UpsertCart(CartHeaderDto cart);
        Task<ResponseDto> RemoveCart(long cartDetailsId);
    }
}
