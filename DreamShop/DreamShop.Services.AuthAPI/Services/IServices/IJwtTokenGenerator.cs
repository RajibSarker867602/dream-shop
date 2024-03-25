using DreamShop.Services.CouponAPI.Models.Entities;

namespace DreamShop.Services.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}
