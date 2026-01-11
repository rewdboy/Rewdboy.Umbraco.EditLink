using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Rewdboy.Umbraco.EditLink;

public class OpenIddictServerEventsHandler :
    IOpenIddictServerHandler<OpenIddictServerEvents.GenerateTokenContext>,
    IOpenIddictServerHandler<OpenIddictServerEvents.ApplyRevocationResponseContext>
{
    public async ValueTask HandleAsync(OpenIddictServerEvents.GenerateTokenContext context)
    {
        // HttpContext finns via OpenIddicts ASP.NET Core integration
        var httpContext = context.Transaction.GetHttpRequest()?.HttpContext;
        if (httpContext is null)
            return;

        // Vi vill bara göra något om det finns en autentiserad principal
        var principal = context.Principal;
        if (principal?.Identity?.IsAuthenticated != true)
            return;

        // Försök plocka ut en stabil identifierare (sub / nameidentifier)
        var userId =
            principal.FindFirst(OpenIddictConstants.Claims.Subject)?.Value
            ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            return;

        // Skapa en "liten" principal för vår cookie
        // (du kan lägga fler claims om du vill, men håll den minimal)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim("editlink", "1")
        };

        // Om principalen har ett name, ta med det (valfritt)
        var name = principal.Identity?.Name
                   ?? principal.FindFirst(ClaimTypes.Name)?.Value;
        if (!string.IsNullOrWhiteSpace(name))
            claims.Add(new Claim(ClaimTypes.Name, name));

        var cookieIdentity = new ClaimsIdentity(claims, EditLinkComposer.Scheme);
        var cookiePrincipal = new ClaimsPrincipal(cookieIdentity);

        // Sätt/refresh cookie via vårt scheme
        await httpContext.SignInAsync(EditLinkComposer.Scheme, cookiePrincipal);
    }

    public async ValueTask HandleAsync(OpenIddictServerEvents.ApplyRevocationResponseContext context)
    {
        var httpContext = context.Transaction.GetHttpRequest()?.HttpContext;
        if (httpContext is null)
            return;

        // När tokens revokas: ta bort vår cookie
        await httpContext.SignOutAsync(EditLinkComposer.Scheme);
    }
}
