using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Notifications;

namespace Rewdboy.Umbraco.EditLink
{
    public class EditLinkAuthCookieNotifications :
        global::Umbraco.Cms.Core.Events.INotificationHandler<UserLoginSuccessNotification>,
        global::Umbraco.Cms.Core.Events.INotificationHandler<UserLogoutSuccessNotification>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditLinkAuthCookieNotifications(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        public void Handle(UserLoginSuccessNotification notification)
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx is null) return;

            ctx.Response.Cookies.Append(
                EditLinkCookie.Name,
                "1",
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,                 // du läser den bara serverside
                    Secure = ctx.Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(12)
                });
        }

        public void Handle(UserLogoutSuccessNotification notification)
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx is null) return;

            ctx.Response.Cookies.Delete(EditLinkCookie.Name, new CookieOptions { Path = "/" });
        }
    }
}
