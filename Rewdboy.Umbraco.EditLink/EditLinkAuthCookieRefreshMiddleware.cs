using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Security;

namespace Rewdboy.Umbraco.EditLink
{
    public class EditLinkAuthCookieRefreshMiddleware : IMiddleware
    {
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
        private readonly EditLinkCookieService _cookieService;

        public EditLinkAuthCookieRefreshMiddleware(
            IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
            EditLinkCookieService cookieService)
        {
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
            _cookieService = cookieService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);

            // Vi vill bara agera på /umbraco-requests (backoffice)
            // (Cookie från backoffice auth skickas där, så IsAuthenticated kan vara sann)
            var path = context.Request.Path.Value ?? "";
            if (!path.StartsWith("/umbraco", StringComparison.OrdinalIgnoreCase))
                return;

            // Om response redan skickad, gör inget
            if (context.Response.HasStarted)
                return;

            var security = _backOfficeSecurityAccessor.BackOfficeSecurity;
            var isAuthed = security?.IsAuthenticated() ?? false;

            if (isAuthed)
            {
                var userId = security?.CurrentUser?.Id ?? 0;
                if (userId > 0)
                {
                    // Refresha signerad cookie på Path="/"
                    _cookieService.SetSignedCookie(context, userId);
                }
            }
            else
            {
                // Om man inte är authed i backoffice → ta bort vår cookie
                _cookieService.DeleteCookie(context);
            }
        }
    }
}
