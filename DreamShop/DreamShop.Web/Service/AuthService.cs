using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;

namespace DreamShop.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> UserRegistration(UserRegistrationRequestDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.AuthAPIBaseUrl + "api/auth/register",
                APIType = APIType.POST,
                Data = requestDto
            });
        }

        public async Task<ResponseDto> UserLogin(UserLoginRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.AuthAPIBaseUrl + "api/auth/login",
                APIType = APIType.POST,
                Data = request
            });
        }

        public async Task<ResponseDto> AssignUserRole(UserRegistrationRequestDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.AuthAPIBaseUrl + "api/auth/assignUserRole",
                APIType = APIType.POST,
                Data = requestDto
            });
        }
    }
}
