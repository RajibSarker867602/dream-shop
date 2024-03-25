using System.Net;
using System.Text;
using DreamShop.Web.Models;
using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using Newtonsoft.Json;


namespace DreamShop.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("DreamShopAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //TODO: set token in headers

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8,
                        "application/json");
                }

                HttpResponseMessage? apiResponse = null;
                switch (requestDto.APIType)
                {
                    case APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not found!" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "You're not authorized to access this request!" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Sorry! this request is forbidden." };
                    case HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, Message = "Invalid input request!" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal server error!" };
                    default:
                        var responseContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
                        return apiResponseDto;
                }
            }
            catch (Exception e)
            {
                return new() { IsSuccess = false, Message = e.Message.ToString() };
            }
        }
    }
}
