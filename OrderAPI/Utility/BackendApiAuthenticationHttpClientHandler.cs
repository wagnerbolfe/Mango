using System;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authentication;

namespace OrderAPI.Utility
{
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await (_accessor.HttpContext ?? throw new InvalidOperationException()).GetTokenAsync("access_token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
