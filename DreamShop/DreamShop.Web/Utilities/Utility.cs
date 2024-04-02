using DreamShop.Web.Models.DTOs;
using Newtonsoft.Json;

namespace DreamShop.Web.Utilities
{
    public class Utility
    {
        public static string CouponAPIBaseUrl { get; set; }
        public static string ProductAPIBaseUrl { get; set; }
        public static string AuthAPIBaseUrl { get; set; }
        public const string AdminRole = "ADMIN";
        public const string CustomerRole = "CUSTOMER";
        public const string TokenCookie = "JwtToken";

        /// <summary>
        /// Deserialize the response result
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static TResponse MapToResponse<TResponse>(object? response)
        {
            return JsonConvert.DeserializeObject<TResponse>(Convert.ToString(response));
        }
    }
}
