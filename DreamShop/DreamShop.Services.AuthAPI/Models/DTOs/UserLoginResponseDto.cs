namespace DreamShop.Services.AuthAPI.Models.DTOs
{
    public class UserLoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }

    }
}
