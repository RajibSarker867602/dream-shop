using System.IdentityModel.Tokens.Jwt;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace DreamShop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequestDto userLoginRequestDto)
        {
            var isLoggedIn = await _authService.UserLogin(userLoginRequestDto);
            if (isLoggedIn.IsSuccess)
            {
                var loginResponse = Utility.MapToResponse<UserLoginResponseDto>(isLoggedIn.Result);
                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);
                TempData["success"] = "User logged in successfully.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", isLoggedIn.Message);
                return View(userLoginRequestDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = GetRoles();
            ViewBag.RoleList = roleList;
            return View(new UserRegistrationRequestDto());
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationRequestDto userRegistrationRequest)
        {
            var userRegisterResult = await _authService.UserRegistration(userRegistrationRequest);
            if (userRegisterResult.IsSuccess)
            {
                if (string.IsNullOrEmpty(userRegistrationRequest.Role))
                {
                    userRegistrationRequest.Role = Utility.CustomerRole;
                }

                var roleAssignResult = await _authService.AssignUserRole(userRegistrationRequest);
                if (roleAssignResult.IsSuccess)
                {
                    TempData["success"] = "User registration successful.";
                    return RedirectToAction(nameof(Login));
                }
            }


            var roleList = GetRoles();
            ViewBag.RoleList = roleList;
            return View(userRegistrationRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            TempData["info"] = "Logout successfully.";
            return RedirectToAction("Index", "Home");
        }

        private static List<SelectListItem> GetRoles()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(text: Utility.AdminRole, value: Utility.AdminRole),
                new SelectListItem(text: Utility.CustomerRole, value: Utility.CustomerRole),
            };
            return roleList;
        }

        private async Task SignInUser(UserLoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));



            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
