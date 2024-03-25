using DreamShop.Services.AuthAPI.Models.DTOs;
using DreamShop.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DreamShop.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        ResponseDto _responseDto;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (requestDto == null) return BadRequest("Invalid input request.");

            var isError = await _authService.UserRegistration(requestDto);
            if (!string.IsNullOrEmpty(isError))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = isError;
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
        {
            var isLoggedIn = await _authService.UserLogin(requestDto);
            if (isLoggedIn.User is null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "User name or password does not matched!";
                return Unauthorized(_responseDto);
            }

            _responseDto.Result = isLoggedIn;
            return Ok(_responseDto);
        }
    }
}
