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
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withToken = true)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("DreamShopAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                
                // set token
                if (withToken)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

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
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
                switch (apiResponse.StatusCode)
                {

                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not found!" };
                    case HttpStatusCode.Unauthorized:
                        return new()
                        {
                            IsSuccess = false,
                            Message = apiResponseDto != null && !string.IsNullOrEmpty(apiResponseDto.Message) ?
                                apiResponseDto.Message : "You're not authorized to access this request!"
                        };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Sorry! this request is forbidden." };
                    case HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, Message = "Invalid input request!" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal server error!" };
                    default:
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
