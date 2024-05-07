using System.Text.Json.Serialization;
using DreamShop.Services.ShoppingCartAPI.IService;
using DreamShop.Services.ShoppingCartAPI.Models.Dtos;
using DreamShop.Services.ShoppingCartAPI.Models.DTOs;
using DreamShop.Services.ShoppingCartAPI.Services.Interface;
using Newtonsoft.Json;

namespace DreamShop.Services.ShoppingCartAPI.Services
{
    public class CouponService: ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<CouponDto> GetCouponByCode(string code)
        {
            var client = _httpClientFactory.CreateClient("coupon");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");

            var response = await client.GetAsync("api/couponapi/" + code + "/code");
            var apiContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (data !=null && data.IsSuccess)
            {
                var coupon = JsonConvert.DeserializeObject<CouponDto>(data.Result.ToString());
                return coupon;
            }

            return new();
        }
    }
}
