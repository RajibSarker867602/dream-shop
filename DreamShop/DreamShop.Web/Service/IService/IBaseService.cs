using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;

namespace DreamShop.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withToken = true);
    }
}
