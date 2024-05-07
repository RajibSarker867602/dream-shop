using DreamShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace DreamShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
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
