using System.Text.Json.Serialization;
using DreamShop.Services.ShoppingCartAPI.Models.Dtos;
using DreamShop.Services.ShoppingCartAPI.Models.DTOs;
using DreamShop.Services.ShoppingCartAPI.Services.Interface;
using Newtonsoft.Json;

namespace DreamShop.Services.ShoppingCartAPI.Services
{
    public class ProductService: IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("product");
            var response = await client.GetAsync("api/ProductAPI");
            var apiContent = await response.Content.ReadAsStringAsync();    
            var data = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (data.IsSuccess)
            {
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(data.Result.ToString());
                return products;
            }

            return new List<ProductDto>();
        }
    }
}
