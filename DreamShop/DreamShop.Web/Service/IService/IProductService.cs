using DreamShop.Web.Models.DTOs;

namespace DreamShop.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProducts();
        Task<ResponseDto> GetProductById(long productId);
        Task<ResponseDto> CreateProduct(ProductDto requestDto);
        Task<ResponseDto> UpdateProduct(ProductDto requestDto);
        Task<ResponseDto> DeleteProduct(long productId);
    }
}
