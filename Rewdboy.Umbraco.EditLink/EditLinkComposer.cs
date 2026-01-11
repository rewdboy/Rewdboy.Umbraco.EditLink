using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace Rewdboy.Umbraco.EditLink
{
    public class EditLinkComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            // =========================
            // 1. Notifications (login/logout)
            // =========================
            builder.AddNotificationHandler<
                global::Umbraco.Cms.Core.Notifications.UserLoginSuccessNotification,
                EditLinkAuthCookieNotifications>();

            builder.AddNotificationHandler<
                global::Umbraco.Cms.Core.Notifications.UserLogoutSuccessNotification,
                EditLinkAuthCookieNotifications>();

            // =========================
            // 2. Services för signerad cookie
            // =========================
            builder.Services.AddSingleton<EditLinkCookieService>();

            // =========================
            // 3. Middleware som refreshar cookien
            // =========================
            builder.Services.AddTransient<EditLinkAuthCookieRefreshMiddleware>();

            // =========================
            // 4. Pipeline filter (utan Program.cs)
            // =========================
            builder.Services.AddUnique<IUmbracoPipelineFilter, EditLinkPipelineFilter>();
        }
    }
}
