using System.Net.Http.Headers;

namespace HealthMonitor.Dashboard.Utilities
{
    public class TokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext;
            
            if (!(httpContext?.User.Identity?.IsAuthenticated ?? false))
                return await base.SendAsync(request, cancellationToken);
         
            var accessToken = httpContext.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

}
