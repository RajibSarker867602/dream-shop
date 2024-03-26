using DreamShop.Web.Service.IService;
using DreamShop.Web.Utilities;

namespace DreamShop.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(Utility.TokenCookie, token);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(Utility.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(Utility.TokenCookie);
        }
    }
}
