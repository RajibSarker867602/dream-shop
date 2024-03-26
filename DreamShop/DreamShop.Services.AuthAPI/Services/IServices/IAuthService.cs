using DreamShop.Services.AuthAPI.Models.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace DreamShop.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> UserRegistration(UserRegistrationRequestDto requestDto);
        Task<UserLoginResponseDto> UserLogin(UserLoginRequestDto request);
        Task<bool> AssignUserRole(string email, string roleName);
    }
}
