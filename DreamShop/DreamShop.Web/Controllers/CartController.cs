using DreamShop.Web.Models;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DreamShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await GetCartForLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartHeaderDto cart)
        {
            var response = await _cartService.AddCoupon(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon added successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Coupon added failed!";
            }

            return View();
        }


        public async Task<IActionResult> Remove(long detailsId)
        {
            var response = await _cartService.RemoveCart(detailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item deleted from cart successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Items cart delete operation failed!";
            }

            return View();
        }

        public async Task<CartHeaderDto> GetCartForLoggedInUser()
        {
            var userId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartService.GetCartsByUserIdAsync(userId);
            if (response != null)
            {
                var data = Utility.MapToResponse<CartHeaderDto>(response.Result);
                return data;
            }

            return new();
        }
    }
}
