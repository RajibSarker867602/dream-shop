using DreamShop.Services.ShoppingCartAPI.Models.Dtos;

namespace DreamShop.Services.ShoppingCartAPI.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
