using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DreamShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new List<ProductDto>();
            var data = await _productService.GetAllProducts();

            if (data != null && data.IsSuccess)
            {
                products = Utility.MapToResponse<List<ProductDto>>(data.Result);
            }
            else
            {
                TempData["error"] = data.Message;
            }

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProduct(product);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Product create operation failed!";
                }
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Update(long productId)
        {
            var existingProduct = await _productService.GetProductById(productId);
            if (existingProduct != null && existingProduct.IsSuccess)
            {
                var productToUpdate = Utility.MapToResponse<ProductDto>(existingProduct.Result);
                return View(productToUpdate);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto product)
        {
            var updateResult = await _productService.UpdateProduct(product);
            if (updateResult != null && updateResult.IsSuccess)
            {
                TempData["success"] = "Product updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Product update operation failed!";
                return NotFound();
            }
        }

        public async Task<IActionResult> Delete(long productId)
        {
            var existingProduct = await _productService.GetProductById(productId);
            if (existingProduct != null && existingProduct.IsSuccess)
            {
                var productToDelete = Utility.MapToResponse<ProductDto>(existingProduct.Result);
                return View(productToDelete);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDto product)
        {
            var existingProduct = await _productService.DeleteProduct(product.Id);
            if (existingProduct != null && existingProduct.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Product delete operation failed!";
                return NotFound();
            }
        }
    }
}
