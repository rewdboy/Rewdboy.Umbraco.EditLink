using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Rewdboy.Umbraco.EditLink;

public class EditLinkComposer : IComposer
{
    public const string Scheme = "EditLinkScheme";
    public const string CookieName = "REWDBOY_EDITLINK";

    public void Compose(IUmbracoBuilder builder)
    {
        // 1) Vår egen cookie auth scheme (som TagHelpern kan AuthenticateAsync på)
        builder.Services
            .AddAuthentication()
            .AddCookie(Scheme, options =>
            {
                options.Cookie.Name = CookieName;
                options.Cookie.Path = "/";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(12);

                // Undvik redirects om någon råkar träffa en endpoint som använder schemat
                options.Events.OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    ctx.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

        // 2) OpenIddict server events handler (som i artikeln)
        builder.Services.AddSingleton<OpenIddictServerEventsHandler>();

        // 3) Koppla handlern till OpenIddict-serverns pipeline
        builder.Services.Configure<OpenIddictServerOptions>(options =>
        {
            options.Handlers.Add(
                OpenIddictServerHandlerDescriptor
                    .CreateBuilder<OpenIddictServerEvents.GenerateTokenContext>()
                    .UseSingletonHandler<OpenIddictServerEventsHandler>()
                    .Build()
            );

            options.Handlers.Add(
                OpenIddictServerHandlerDescriptor
                    .CreateBuilder<OpenIddictServerEvents.ApplyRevocationResponseContext>()
                    .UseSingletonHandler<OpenIddictServerEventsHandler>()
                    .Build()
            );
        });
    }
}
