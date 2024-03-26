using DreamShop.Web.Models.DTOs;

namespace DreamShop.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> UserRegistration(UserRegistrationRequestDto requestDto);
        Task<ResponseDto> UserLogin(UserLoginRequestDto request);
        Task<ResponseDto> AssignUserRole(UserRegistrationRequestDto requestDto);
    }
}
