using DreamShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DreamShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> _products = new();
            var products = await _productService.GetAllProducts();
            if (products != null && products.IsSuccess)
            {
                _products = Utility.MapToResponse<List<ProductDto>>(products.Result);
            }
            else
            {
                TempData["error"] = products.Message;
            }
            return View(_products);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(long productId)
        {
            ProductDto _product = new();
            var response = await _productService.GetProductById(productId);
            if (response != null && response.IsSuccess)
            {
                _product = Utility.MapToResponse<ProductDto>(response.Result);
            }

            return View(_product);  
        }

        [HttpPost]
        [Authorize]
        [ActionName("Details")]
        public async Task<IActionResult> Details(ProductDto productDto)
        {
            CartHeaderDto cart = new CartHeaderDto()
            {
                UserId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value,
                CartDetails = new List<CartDetailsDto>()
                {
                    new CartDetailsDto()
                    {
                        ProductId = productDto.Id,
                        Count = productDto.Count
                    }
                }
            };

            var response = await _cartService.UpsertCart(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item successfully added to cart.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Item could not be added to cart!";
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
