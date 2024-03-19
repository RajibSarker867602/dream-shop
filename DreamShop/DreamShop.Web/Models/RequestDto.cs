namespace DreamShop.Web.Models
{
    public enum APIType
    {
        GET = 1,
        POST = 2,
        PUT = 3,
        DELETE = 4,
    }
    public class RequestDto
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object? Data { get; set; }
        public string AccessToken { get; set; }
    }
}
