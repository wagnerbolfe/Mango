using Microsoft.AspNetCore.Http;
using Web.Utility;

namespace Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StartDetail.TokenCookie);
        }

        public string GetToken()
        {
            string token = null;
            var hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StartDetail.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(StartDetail.TokenCookie, token);
        }
    }
}
