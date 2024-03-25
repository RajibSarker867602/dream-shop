namespace DreamShop.Services.AuthAPI.Models.DTOs
{
    public class UserRegistrationRequestDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }


    }
}
