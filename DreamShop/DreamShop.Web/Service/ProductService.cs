using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;

namespace DreamShop.Web.Service
{
    public class ProductService: IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> GetAllProducts()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.ProductAPIBaseUrl + "api/productapi",
                APIType = APIType.GET
            });
        }

        public async Task<ResponseDto> GetProductById(long productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.ProductAPIBaseUrl + "api/productapi/" + productId,
                APIType = APIType.GET
            });
        }

        public async Task<ResponseDto> CreateProduct(ProductDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.ProductAPIBaseUrl + "api/productapi",
                APIType = APIType.POST,
                Data = requestDto
            });
        }

        public async Task<ResponseDto> UpdateProduct(ProductDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.ProductAPIBaseUrl + "api/productapi",
                APIType = APIType.PUT,
                Data = requestDto
            });
        }

        public async Task<ResponseDto> DeleteProduct(long productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = Utility.ProductAPIBaseUrl + "api/productapi/" + productId,
                APIType = APIType.DELETE
            });
        }
    }
}
