using DreamShop.Services.AuthAPI.Data;
using DreamShop.Services.AuthAPI.Models.DTOs;
using DreamShop.Services.AuthAPI.Services.IServices;
using DreamShop.Services.CouponAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace DreamShop.Services.AuthAPI.Services
{
    public class AuthService: IAuthService
    {
        private readonly AuthAPIDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AuthAPIDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> UserRegistration(UserRegistrationRequestDto requestDto)
        {
            try
            {
                var userToRegistration = new ApplicationUser()
                {
                    Name = requestDto.Name,
                    Email = requestDto.Email,
                    NormalizedEmail = requestDto.Email.ToUpper(),
                    UserName = requestDto.Email,
                    PhoneNumber = requestDto.PhoneNumber
                };

                var result = await _userManager.CreateAsync(userToRegistration, requestDto.Password);
                if (result.Succeeded)
                {
                    var data = await _db.Users.FirstOrDefaultAsync(c => c.UserName == requestDto.Email);
                    //return new UserDto
                    //{
                    //    Name = data.Name,
                    //    PhoneNumber = data.PhoneNumber,
                    //    Id = data.Id,
                    //    Email = data.Email
                    //};

                    return data.UserName;
                }

                return result.Errors.FirstOrDefault().Description;
            }
            catch (Exception e)
            {
                //return e.Message;
            }

            return "User registration failed!";
        }

        public async Task<UserLoginResponseDto> UserLogin(UserLoginRequestDto request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(c => c.UserName.ToLower() == request.UserName.ToLower());
            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (user is null || !isValid) return new UserLoginResponseDto() { User = null, Token = "" };

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new UserLoginResponseDto()
            {
                User = new()
                {
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Id = user.Id
                },
                Token = token
            };
        }
    }
}
